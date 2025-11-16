namespace Pxl.Ui.CSharp;

public static class Simulator
{
    public static Task Start(string host, Action myScene)
    {
        Pxl.Ui.Simulator.startAction(
            CanvasProxy.createWithDefaults(host),
            myScene);
        if (Pxl.ApiEnv.isInInteractiveContext)
            Console.WriteLine("Simulator started. Press any key to exit...");
        return Task.Delay(Timeout.Infinite);
    }
}
