namespace Pxl.Ui.CSharp;

public partial class DrawingContext
{
    private static RenderCtx GetRenderCtx() =>
        RenderCtxInstances.RenderContexts.TryGetValue(Environment.CurrentManagedThreadId, out var ctx)
            ? ctx
            : throw new Exception($"No RenderCtx associated with the current thread (MTID {Environment.CurrentManagedThreadId}).");

    public static DrawingContext Ctx => new();

    public DrawingContext(RenderCtx? renderCtx = null)
    {
        RenderCtx = renderCtx ?? GetRenderCtx();
    }

    public readonly RenderCtx RenderCtx;
}
