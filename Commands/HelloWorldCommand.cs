using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;
using SpectreConsoleAppTemplate.Spectre;

namespace SpectreConsoleAppTemplate.Commands
{
    public class HelloWorldCommand : AsyncCommand<SharedSpectreCommandSettings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, SharedSpectreCommandSettings settings)
        {
            // Synchronous
            await AnsiConsole.Status()
                .StartAsync("Thinking...", async ctx =>
                {
                    // Simulate some work
                    AnsiConsole.MarkupLine("Doing some work...");
                    Thread.Sleep(500);

                    // Update the status and spinner
                    ctx.Status("Thinking some more");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    // Simulate some work
                    AnsiConsole.MarkupLine("Doing some more work...");
                    Thread.Sleep(1000);

                    // throw exception as a test of exception handling
                    // throw new InvalidOperationException("oh noooo...");
                });
            return 0;
        }
    }
}