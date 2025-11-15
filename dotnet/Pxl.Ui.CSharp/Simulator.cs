namespace Pxl.Ui.CSharp;

public static class Simulator
{
    public static void Start(string host, Action myScene)
    {
        Pxl.Simulator.startAction(
            CanvasProxy.createWithDefaults(host),
            myScene);
        Console.WriteLine("Simulator started. Press any key to exit...");
        Console.ReadKey();
    }
}
