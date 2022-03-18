using System.Diagnostics;

namespace Vival;

internal static class LinuxUtils
{
    #region Variables
    /// <summary>
    /// <see cref="ProcessStartInfo"/> info for <see cref="GetConsoleOut"/> and <see cref="ExecuteCommand"/>.
    /// </summary>
    private static readonly ProcessStartInfo executeCommand = new();

    /// <summary>
    /// Custom bash process for executing stdin commands.
    /// </summary>
    private static Process? customProcess;

    /// <summary>
    /// New line character.
    /// </summary>
    private const char split = '\n';
    #endregion

    /// <summary>
    /// Setup our custom process which will be used to send commands.
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public static void SetupProcess()
    {
        var info = new ProcessStartInfo
        {
            FileName = "/usr/bin/bash",
            CreateNoWindow = true,
            RedirectStandardInput = true,
            UseShellExecute = false
        };
        customProcess = Process.Start(info) ?? throw new NullReferenceException("CUSTOMPROCESS IS NULL");
    }

    /// <summary>
    /// Get the output from a command.
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string GetConsoleOut(string cmd, string args)
    {
        executeCommand.FileName = cmd;
        executeCommand.Arguments = args;
        executeCommand.CreateNoWindow = true;
        executeCommand.RedirectStandardOutput = true;
        executeCommand.UseShellExecute = false;
        using var proc = Process.Start(executeCommand);
        var stdOut = proc?.StandardOutput.ReadToEnd();
        proc?.StandardOutput.Close();
        return stdOut?.Split(split)[0] ?? string.Empty;
    }

    /// <summary>
    /// Executes a command.
    /// </summary>
    /// <param name="cmd"></param>
    public static void ExecuteCommand(string cmd) => customProcess?.StandardInput.WriteLine($"bash -c \"{cmd}\"");
}
