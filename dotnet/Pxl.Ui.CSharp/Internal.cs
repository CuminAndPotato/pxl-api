namespace Pxl.Ui.CSharp.Internal;

using Pxl;

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
