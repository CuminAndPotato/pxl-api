namespace Pxl.Ui.CSharp;

using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.FSharp.Core;
using Pxl;
using SkiaSharp;

public record struct PixelCell
{
    public required int X { get; init; }
    public required int Y { get; init; }
    public required SKColor Color { get; init; }
}

public sealed class PixelsAccess
{
    private static readonly Dictionary<RenderCtx, SKColor[]> ctxToPixelSpan = new();

    private readonly int _width;
    private readonly int _height;
    private readonly RenderCtx _ctx;

    internal PixelsAccess(RenderCtx ctx)
    {
        _ctx = ctx;
        _width = ctx.Width;
        _height = ctx.Height;
    }

    private SKColor[] GetPixels()
    {
        if (!ctxToPixelSpan.TryGetValue(_ctx, out var pixels))
        {
            var dim = _ctx.Width * _ctx.Height;
            pixels = new SKColor[dim];
            ctxToPixelSpan[_ctx] = pixels;
        }

        if (FSharpOption<IDirectDrawable>.get_IsSome(_ctx.CurrentDirectDrawable)
            && _ctx.CurrentDirectDrawable.Value is PixelsDrawOperation op)
        {
            return op.Pixels;
        }

        _ctx.FlushAndCopy(pixels);

        _ctx.BeginDirectDrawable(new PixelsDrawOperation(pixels));

        return pixels;
    }

    public SKColor this[int index]
    {
        get => GetPixels()[index];
        set => GetPixels()[index] = value;
    }

    public SKColor this[int x, int y]
    {
        get => GetPixels()[y * _width + x];
        set => GetPixels()[y * _width + x] = value;
    }

    public int Length => GetPixels().Length;
    public int Width => _width;
    public int Height => _height;

    public IEnumerable<PixelCell> Cells
    {
        get
        {
            var pixels = GetPixels();
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    yield return new PixelCell
                    {
                        X = x,
                        Y = y,
                        Color = pixels[y * _width + x]
                    };
                }
            }
        }
    }
}

public static class RenderCtxExtensions
{
    public static RenderCtx Fork(this RenderCtx ctx, int width, int height, SKColor clearColor)
    {
        var forked = new RenderCtx(width, height, ctx.Fps, FSharpOption<FSharpFunc<Pxl.Color[], Unit>>.None)
        {
            _buttons = ctx._buttons,
            _startTime = ctx._startTime,
            _now = ctx._now
        };
        forked.SkiaCanvas.Clear(clearColor);
        return forked;
    }

    public static void ApplyPixels(
        this RenderCtx ctx,
        SKColor[] pixels,
        bool flushFirst,
        double x,
        double y,
        SKBlendMode blendMode)
    {
        if (flushFirst)
            ctx.Flush();

        var byteSpan = MemoryMarshal.Cast<SKColor, byte>(pixels);
        byteSpan.CopyTo(ctx.SkiaBitmap.GetPixelSpan());

        using var paint = new SKPaint { BlendMode = blendMode };
        ctx.SkiaCanvas.DrawBitmap(ctx.SkiaBitmap, (float)x, (float)y, paint);
        ctx.SkiaCanvas.Flush();
    }
}

public sealed class PixelsDrawOperation : IDirectDrawable
{
    internal readonly SKColor[] Pixels;

    internal PixelsDrawOperation(SKColor[] pixels)
    {
        Pixels = pixels;
    }

    public double X { get; set; } = 0;
    public double Y { get; set; } = 0;
    public BlendMode BlendMode { get; set; } = BlendMode.SourceOver;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void End(RenderCtx ctx)
    {
        ctx.ApplyPixels(Pixels, false, X, Y, (SKBlendMode)BlendMode);
    }
}

public static class PixelDrawOperationExtensions
{
    public static PixelsDrawOperation X(this PixelsDrawOperation op, double x)
    {
        op.X = x;
        return op;
    }

    public static PixelsDrawOperation Y(this PixelsDrawOperation op, double y)
    {
        op.Y = y;
        return op;
    }
}

public partial class DrawingContext
{
    public PixelsAccess Pixels => new(RenderCtx);
}

public class ForkedDrawingContext : DrawingContext
{
    private readonly RenderCtx _parent;
    private readonly RenderCtx _forked;

    public ForkedDrawingContext(RenderCtx parent, RenderCtx forked)
        : base(forked)
    {
        _parent = parent;
        _forked = forked;
    }

    public void Apply(double x, double y, SKBlendMode blendMode)
    {
        var temp = new SKColor[_forked.Width * _forked.Height];
        _forked.FlushAndCopy(temp);
        _parent.ApplyPixels(temp, true, x, y, blendMode);
    }
}

public static class RenderCtxPixelsExtensions
{
    /// <summary>
    /// Creates a new RenderCtx initialized with a cropped region from the parent context.
    /// If x, y, width, or height is null, defaults are used (0, 0, ctx.Width, ctx.Height).
    /// The forked context has to be applied back to the parent context manually using Apply().
    /// </summary>
    public static ForkedDrawingContext Fork(
        this DrawingContext ctx,
        int? x = null,
        int? y = null,
        int? width = null,
        int? height = null)
    {
        var x0 = x ?? 0;
        var y0 = y ?? 0;
        var w = width ?? ctx.RenderCtx.Width;
        var h = height ?? ctx.RenderCtx.Height;

        var forked = ctx.RenderCtx.Fork(w, h, SKColors.Transparent);

        var parentPixels = new SKColor[ctx.RenderCtx.Width * ctx.RenderCtx.Height];
        ctx.RenderCtx.FlushAndCopy(parentPixels);

        var forkedPixels = new SKColor[w * h];
        for (var row = 0; row < h; row++)
        {
            Array.Copy(
                parentPixels,
                (y0 + row) * ctx.RenderCtx.Width + x0,
                forkedPixels,
                row * w,
                w);
        }

        forked.ApplyPixels(forkedPixels, false, 0, 0, SKBlendMode.Src);

        return new ForkedDrawingContext(ctx.RenderCtx, forked);
    }

    /// <summary>
    /// Creates a new black RenderCtx.
    /// If width or height is null, the current context's dimension is used.
    /// The forked context has to be applied back to the parent context manually using Apply().
    /// </summary>
    public static ForkedDrawingContext Spawn(
        this DrawingContext ctx,
        int? width = null,
        int? height = null,
        SKColor? clearColor = null)
    {
        var forked = ctx.RenderCtx.Fork(
            width ?? ctx.RenderCtx.Width,
            height ?? ctx.RenderCtx.Height,
            clearColor ?? SKColors.Black);
        return new ForkedDrawingContext(ctx.RenderCtx, forked);
    }
}
