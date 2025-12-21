namespace Pxl.Ui.CSharp;

using Microsoft.FSharp.Core;

public static class PXL
{
    private static async Task Run(
        Action myScene,
        string primaryDeviceAddress,
        params string[] secondaryDeviceAddresses
        )
    {
        if (!ApiEnv.isInInteractiveContext)
            return;
        
        Simulator.startActionSimple(
            primaryDeviceAddress,
            secondaryDeviceAddresses,
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

    const string SimulateAddress = "127.0.0.1";

    public static async Task Simulate(Action myScene) =>
        await Run(myScene, SimulateAddress);

    public static async Task SendToDevice(Action myScene, string deviceAddress) =>
        await Run(myScene, deviceAddress);

    public static async Task SimulateAndSendToDevice(Action myScene, string deviceAddress) =>
        await Run(myScene, SimulateAddress, deviceAddress);
}
