using System.ComponentModel;
using Spectre.Console.Cli;

namespace SpectreConsoleAppTemplate.Spectre
{
    public class SharedSpectreCommandSettings : CommandSettings
    {
        [CommandOption("-w|--wait-for-debugger", IsHidden = true)]
        [Description("When specified it blocks execution until the Visual Studio debugger is attached.")]
        public bool WaitForDebugger { get; set; }
    }
}