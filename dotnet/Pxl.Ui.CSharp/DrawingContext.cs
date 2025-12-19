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

    internal readonly RenderCtx RenderCtx;

    public double WidthF => RenderCtx.WidthF;
    public double HeightF => RenderCtx.HeightF;
    public int Width => RenderCtx.Width;
    public int Height => RenderCtx.Height;
    public DateTimeOffset Now => RenderCtx.Now;
    public long CycleNo => RenderCtx.CycleNo;
    public TimeSpan Elapsed => RenderCtx.Elapsed;
    public int Fps => RenderCtx.Fps;
}
