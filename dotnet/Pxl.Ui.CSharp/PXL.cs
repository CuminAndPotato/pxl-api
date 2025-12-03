namespace Pxl.Ui.CSharp;

using Microsoft.FSharp.Core;

public static class PXL
{
    private static async Task Run(
        Action myScene,
        string? deviceAddress = null)
    {
        if (!ApiEnv.isInInteractiveContext)
            return;

        Simulator.startActionSimple(
            string.IsNullOrEmpty(deviceAddress)
                ? FSharpOption<string>.None
                : FSharpOption<string>.Some(deviceAddress),
            myScene);

        Console.WriteLine("Simulator started ...");
        Console.WriteLine("Press Ctrl+C to stop...");

        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        try
        {
            await Task.Delay(Timeout.Infinite, cts.Token);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Stopping simulator...");
        }
    }

    public static async Task Simulate(Action myScene) =>
        await Run(myScene, null);

    public static async Task SendToDevice(Action myScene, string deviceAddress) =>
        await Run(myScene, deviceAddress);
}
