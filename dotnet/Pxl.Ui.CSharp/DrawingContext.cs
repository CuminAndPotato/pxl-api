namespace Pxl.Ui.CSharp;

/// <summary>
/// This class is intended to be "using static"-ed to provide easy access to the current RenderCtx
/// and to other basic idiomatic PXL functions.
/// </summary>
public partial class DrawingContext
{
    private static RenderCtx GetRenderCtx() =>
        RenderCtxInstances.RenderContexts.TryGetValue(Environment.CurrentManagedThreadId, out var ctx)
            ? ctx
            : throw new Exception($"No RenderCtx associated with the current thread (MTID {Environment.CurrentManagedThreadId}).");

    public DrawingContext(RenderCtx? renderCtx = null)
    {
        RenderCtx = renderCtx ?? GetRenderCtx();
    }

    public static DrawingContext Ctx => new();

    public static IEnumerable<(int, int)> Grid(int size)
    {
        foreach (var x in Enumerable.Range(0, size))
            foreach (var y in Enumerable.Range(0, size))
                yield return (x, y);
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
