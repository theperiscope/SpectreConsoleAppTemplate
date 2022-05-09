using System.ComponentModel;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;
using SpectreConsoleAppTemplate.CLI;
using SpectreConsoleAppTemplate.Spectre;

namespace SpectreConsoleAppTemplate.Commands
{
    internal class GetPublicHolidaysCommand : AsyncCommand<GetPublicHolidaysCommand.Settings>
    {
        private readonly IPublicHolidayAPI _api;

        public GetPublicHolidaysCommand(IPublicHolidayAPI api)
        {
            _api = api ?? throw new System.ArgumentNullException(nameof(api));
        }

        internal sealed class Settings : SharedSpectreCommandSettings
        {
            [CommandArgument(0, "<Year>")]
            [Description("Year to get public holidays for")]
            public int Year { get; set; }

            [CommandArgument(1, "<CountryCode>")]
            [Description("ISO 3166-1 alpha-2")]
            public string CountryCode { get; set; }

            public override ValidationResult Validate()
            {
                if (Year < 1922)
                {
                    return ValidationResult.Error("Year is not supported.");
                }
                if (string.IsNullOrEmpty(CountryCode) || CountryCode.Length != 2)
                {
                    return ValidationResult.Error("CountryCode must be a valid ISO 3166-1 alpha-2 code.");
                }
                return ValidationResult.Success();
            }
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var holidays = await _api.Get(settings.Year, settings.CountryCode);

            var table = new Table();
            table.AddColumn("Name");
            table.AddColumn(new TableColumn("Date"));

            foreach (var holiday in holidays)
            {
                table.AddRow(Markup.FromInterpolated($"{holiday.Name}"), Markup.FromInterpolated($"[bold yellow]{holiday.Date}[/]"));
            }

            AnsiConsole.Write(table);

            return 0;
        }
    }
}