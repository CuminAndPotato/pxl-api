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

public static class Simulator
{
    public static void Start(string address, Action scene)
    {
        Pxl.PxlLocalDev.Simulator.startWith(address, scene);
    }
}
