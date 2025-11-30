namespace Pxl.Ui.CSharp;

using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.FSharp.Core;
using Pxl;
using SkiaSharp;

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

    private SKColor[] Pixels()
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
        get => Pixels()[index];
        set => Pixels()[index] = value;
    }

    public SKColor this[int x, int y]
    {
        get => Pixels()[y * _width + x];
        set => Pixels()[y * _width + x] = value;
    }

    public int Length => Pixels().Length;
    public int Width => _width;
    public int Height => _height;
}

public sealed class PixelsDrawOperation : IDirectDrawable
{
    internal readonly SKColor[] Pixels;

    internal PixelsDrawOperation(SKColor[] pixels)
    {
        Pixels = pixels;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void End(RenderCtx ctx)
    {
        // Write data back to canvas
        var byteSpan = MemoryMarshal.Cast<SKColor, byte>(Pixels);
        byteSpan.CopyTo(ctx.SkiaBitmap.GetPixelSpan());

        ctx.SkiaCanvas.Clear();
        ctx.SkiaCanvas.DrawBitmap(ctx.SkiaBitmap, 0.0f, 0.0f);
        ctx.SkiaCanvas.Flush();
    }
}

public partial class DrawingContext
{
    public PixelsAccess Pixels => new(RenderCtx);
}
