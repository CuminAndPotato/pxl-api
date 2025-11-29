using System.Diagnostics;

namespace Pxl.Ui.CSharp;

// class AsyncDisposableAction(Func<ValueTask> disposeAction) : IAsyncDisposable
// {
//     private readonly Func<ValueTask> _disposeAction = disposeAction;
//     public ValueTask DisposeAsync() => _disposeAction();
// }

public static class PXL
{
    private const string SimulatorVersion = "0.0.13";

    private static readonly string SimulatorUrl = $"http://127.0.0.1:{CanvasProxy.InvariantServicePorts.http}";

    private static async Task<List<Process>> StartSimulator()
    {
        var processes = new List<Process>();

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
        processes.Add(installProcess);

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
            processes.Add(updateProcess);
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
        processes.Add(simulatorProcess);

        // HACK / TODO: Wait for simulator to start
        await Task.Delay(2000);

        return processes;
    }

    public static async Task Run(
        string host,
        bool startSimulator,
        bool openSimulatorGuiInBrowser,
        Action myScene)
    {
        if (!ApiEnv.isInInteractiveContext)
            return;

        List < Process > processes = new();

        // Start simulator
        if (startSimulator)
            processes.AddRange(await StartSimulator());

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

        Simulator.startAction(
            CanvasProxy.createWithDefaults(host),
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
        catch (OperationCanceledException)
        {
            Console.WriteLine("Shutting down...");

            foreach (var process in processes)
            {
                try
                {
                    Console.WriteLine($"Stopping process {process.Id}...");

                    if (!process.HasExited)
                    {
                        process.Kill();
                        using var killCts = new CancellationTokenSource(1000);
                        await process.WaitForExitAsync(killCts.Token);
                    }
                    process.Dispose();
                }
                catch (OperationCanceledException)
                {
                    // Process didn't exit within timeout
                    Console.WriteLine($"Process {process.Id} did not exit within timeout");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error disposing process: {ex.Message}");
                }
            }
        }
    }
}
