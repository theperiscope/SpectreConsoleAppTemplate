using System;
using System.Diagnostics;
using System.Threading;
using Spectre.Console.Cli;
using SpectreConsoleAppTemplate.Spectre;

namespace SpectreConsoleAppTemplate.Framework
{
    /// <summary>
    /// Spectre command interceptor to wait until Visual Studio Debugger is attached.
    /// </summary>
    public class WaitForDebuggerInterceptor : ICommandInterceptor
    {
        public void Intercept(CommandContext context, CommandSettings settings)
        {
            if (!(settings is SharedSpectreCommandSettings baseSettings) || !baseSettings.WaitForDebugger) return;

            Console.WriteLine("Waiting for the Debugger to be attached...");
            while (!Debugger.IsAttached)
            {
                Thread.Sleep(400);
            }
        }
    }
}