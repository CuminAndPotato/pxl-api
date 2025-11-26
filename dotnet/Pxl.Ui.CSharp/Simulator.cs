using System.Diagnostics;

namespace Pxl.Ui.CSharp;

public static class Simulator
{
    private const string SimulatorVersion = "0.0.13";

    private static readonly string SimulatorUrl = $"http://127.0.0.1:{CanvasProxy.InvariantServicePorts.http}";

    public static Task Send(string host, Action myScene)
    {
        Pxl.Ui.Simulator.startAction(
            CanvasProxy.createWithDefaults(host),
            myScene);
        if (Pxl.ApiEnv.isInInteractiveContext)
            Console.WriteLine("Simulator started ...");
        return Task.Delay(Timeout.Infinite);
    }

    public static async Task Run(string host, bool openSimulatorGuiInBrowser, Action myScene)
    {
        // ------------------------
        // Install/update simulator
        // ------------------------

        Console.WriteLine("Installing / updating PXL simulator ...");

        var installProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"tool install --global Pxl.Simulator --version {SimulatorVersion}",
                UseShellExecute = false
            }
        };
        installProcess.Start();
        installProcess.WaitForExit();

        if (installProcess.ExitCode != 0)
        {
            var updateProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"tool update --global Pxl.Simulator --version {SimulatorVersion}",
                    UseShellExecute = false
                }
            };
            updateProcess.Start();
            updateProcess.WaitForExit();
        }

        // Start simulator process
        Console.WriteLine("Starting simulator...");
        var simulatorProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "Pxl.Simulator",
                UseShellExecute = true
            }
        };
        simulatorProcess.Start();

        // HACK: Wait for simulator to start
        await Task.Delay(2000);

        // Open browser
        if (openSimulatorGuiInBrowser)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "open",
                Arguments = SimulatorUrl,
                UseShellExecute = true
            });
        }

        await Send(host, myScene);

        // TODO: Terminate simulator on exit?
    }
}
