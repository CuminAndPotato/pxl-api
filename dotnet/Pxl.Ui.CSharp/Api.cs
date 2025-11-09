namespace PxlClock;

using Pxl;

public static class Drawing
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
