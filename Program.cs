using System;
using System.Diagnostics;
using Avalonia;

namespace Vival;

internal static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // ? If Vival was already opened, close it. Acting as a toggle behavior.
        if (Process.GetProcessesByName("Vival") is not {Length: 1})
        {
            KillVival();
            return;
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    /// <summary>
    /// Kills all Vival instances.
    /// </summary>
    private static void KillVival()
    {
        var proc = Process.GetProcessesByName("Vival");
        for (var i = 0; i < proc.Length; i++) proc[i].Kill();
        Environment.Exit(0);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}