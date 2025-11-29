namespace Pxl.Ui.CSharp.Internal;

using Pxl;
using SkiaSharp;

public class DrawingFunctions(RenderCtx ctx)
{
    public DrawingContext Context() => new(ctx);
}

public class DrawingContext(RenderCtx ctx)
{
    class NoOperation : IDirectDrawable { public void End(RenderCtx value) { } }

    private IDirectDrawable _builder = new NoOperation();

    private T Begin<T>(Func<T> factory) where T : IDirectDrawable
    {
        _builder.End(ctx);
        var builder = factory();
        _builder = builder;
        return builder;
    }
}

public static class SkiaExtensions
{
    public static SKPoint ToSkiaPoint(this (double x, double y) tuple) => new((float)tuple.x, (float)tuple.y);
    public static (double x, double y) ToTuple(this SKPoint point) => (point.X, point.Y);
}