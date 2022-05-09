using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;
using SpectreConsoleAppTemplate.Spectre;

namespace SpectreConsoleAppTemplate.Commands
{
    internal sealed class ShowCurrentDateTimeCommand : AsyncCommand<ShowCurrentDateTimeCommand.Settings>
    {
        internal sealed class Settings : SharedSpectreCommandSettings
        {
            [CommandOption("-f|--format", IsHidden = false)]
            [Description(".NET format string for displayed date/time")]
            [DefaultValue("HH:MM")]
            public string Format { get; set; }

            [CommandOption("-u|--utc")]
            [Description("Controls whether to use local (default) or UTC time")]
            [DefaultValue(false)]
            public bool IsUtc { get; set; }

            public override ValidationResult Validate()
            {
                try
                {
                    var s = DateTime.Now.ToString(Format, CultureInfo.InvariantCulture);
                    DateTime.ParseExact(s, Format, CultureInfo.InvariantCulture);
                    return ValidationResult.Success();
                }
                catch
                {
                    return ValidationResult.Error("Unable to parse specified format.");
                }
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task<int> ExecuteAsync(CommandContext context, ShowCurrentDateTimeCommand.Settings settings)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var dateTime = settings?.IsUtc == true ? DateTime.UtcNow : DateTime.Now;
            AnsiConsole.MarkupLine("[bold yellow]{0}[/]", dateTime.ToString(settings.Format));
            return 0;
        }
    }
}