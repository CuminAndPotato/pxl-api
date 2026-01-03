namespace Pxl.Ui.CSharp;

using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.FSharp.Core;
using Pxl;
using SkiaSharp;

public record PixelCell(int X, int Y, SKColor Color);

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
                    yield return new PixelCell(x, y, pixels[y * _width + x]);
            }
        }
    }
}

public static class RenderCtxExtensions
{
    public static RenderCtx Fork(this RenderCtx ctx, SKColor clearColor)
    {
        var forked = new RenderCtx(ctx.Width, ctx.Height, ctx.Fps, FSharpOption<FSharpFunc<Pxl.Color[], FSharpFunc<RenderCtx, Unit>>>.None)
        {
            _buttons = ctx._buttons,
            _startTime = ctx._startTime,
            _now = ctx._now
        };
        forked.SkiaCanvas.Clear(clearColor);
        return forked;
    }

    public static void SetPixels(
        this RenderCtx ctx,
        SKColor[] pixels,
        bool flushFirst,
        SKBlendMode blendMode)
    {
        if (flushFirst)
            ctx.Flush();

        var byteSpan = MemoryMarshal.Cast<SKColor, byte>(pixels);
        byteSpan.CopyTo(ctx.SkiaBitmap.GetPixelSpan());

        using var paint = new SKPaint { BlendMode = blendMode };
        ctx.SkiaCanvas.DrawBitmap(ctx.SkiaBitmap, 0, 0, paint);
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

    public BlendMode BlendMode { get; set; } = BlendMode.Source;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void End(RenderCtx ctx)
    {
        ctx.SetPixels(Pixels, false, (SKBlendMode)BlendMode);
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

    public void Apply(SKBlendMode blendMode)
    {
        var temp = new SKColor[_forked.Width * _forked.Height];
        _forked.FlushAndCopy(temp);
        _parent.SetPixels(temp, true, blendMode);
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
        this DrawingContext ctx)
    {
        var forked = ctx.RenderCtx.Fork(SKColors.Transparent);

        var parentPixels = new SKColor[ctx.RenderCtx.Width * ctx.RenderCtx.Height];
        ctx.RenderCtx.FlushAndCopy(parentPixels);

        var forkedPixels = new SKColor[forked.Width * forked.Height];
        for (var row = 0; row < forked.Height; row++)
        {
            Array.Copy(
                parentPixels,
                row * ctx.RenderCtx.Width,
                forkedPixels,
                row * forked.Width,
                forked.Width);
        }

        forked.SetPixels(forkedPixels, false, SKBlendMode.Src);

        return new ForkedDrawingContext(ctx.RenderCtx, forked);
    }

    /// <summary>
    /// Creates a new black RenderCtx.
    /// If width or height is null, the current context's dimension is used.
    /// The forked context has to be applied back to the parent context manually using Apply().
    /// </summary>
    public static ForkedDrawingContext Spawn(
        this DrawingContext ctx,
        SKColor? clearColor = null)
    {
        var forked = ctx.RenderCtx.Fork(clearColor ?? SKColors.Black);
        return new ForkedDrawingContext(ctx.RenderCtx, forked);
    }

    /// <summary>
    /// Draws the given pixel array onto the canvas.
    /// The array length must match Width * Height of the context.
    /// </summary>
    public static void SetPixels(
        this DrawingContext ctx,
        SKColor[] pixels,
        BlendMode blendMode = BlendMode.Source)
    {
        ctx.RenderCtx.SetPixels(pixels, flushFirst: true, (SKBlendMode)blendMode);
    }
}
