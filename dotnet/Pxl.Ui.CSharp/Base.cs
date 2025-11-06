namespace Pxl.Ui;

using Pxl;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class BuilderStyleAttribute : Attribute { public BuilderStyleAttribute() { } }

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

public static class PxlClock
{
    public static RenderCtx Ctx
    {
        get
        {
            var threadId = Environment.CurrentManagedThreadId;
            if (RenderCtxInstances.RenderContexts.TryGetValue(threadId, out var ctx))
                return ctx;
            throw new Exception($"No RenderCtx associated with the current thread (MTID {threadId}).");
        }
    }
}
