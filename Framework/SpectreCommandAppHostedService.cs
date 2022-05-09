using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;
using SpectreConsoleAppTemplate.Commands;

namespace SpectreConsoleAppTemplate.Framework
{
    /// <summary>
    /// <see cref="SpectreCommandAppHostedService"/> runs Spectre App using the provided command-line
    /// arguments and stops application host afterwards.
    /// </summary>
    public class SpectreCommandAppHostedService : IHostedService
    {
        private readonly ILogger<SpectreCommandAppHostedService> logger;
        private readonly IHostApplicationLifetime appLifetime;
        private readonly ITypeRegistrar typeRegistrar;

        private CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        private TaskCompletionSource<bool> TaskCompletionSource { get; } = new TaskCompletionSource<bool>();

        public SpectreCommandAppHostedService(ILogger<SpectreCommandAppHostedService> logger, IHostApplicationLifetime appLifetime, ITypeRegistrar typeRegistrar)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            this.typeRegistrar = typeRegistrar ?? throw new ArgumentNullException(nameof(typeRegistrar));
            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogTrace("1. StartAsync has been called.");

            Task.Run(() => DoWork(CancellationTokenSource.Token), cancellationToken);

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            logger.LogInformation("2. OnStarted has been called.");
        }

        private void OnStopping()
        {
            logger.LogTrace("3. OnStopping has been called.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogTrace("4. StopAsync has been called.");

            CancellationTokenSource.Cancel();

            return Task.CompletedTask;
        }

        private void OnStopped()
        {
            logger.LogTrace("5. OnStopped has been called.");
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            var app = new CommandApp(typeRegistrar);

            // uncomment to set a default command
            // app.SetDefaultCommand<ShowCurrentDateTimeCommand>();

            app.Configure(configurator =>
            {
                // without setting application name the .dll is rendered with extension (even if .exe is built) when --help is used
                configurator.SetApplicationName(System.IO.Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName));

                configurator.ValidateExamples();

                configurator.AddCommand<HelloWorldCommand>("hello-world");
                configurator.AddCommand<ShowCurrentDateTimeCommand>("now")
                    .WithExample(new[] { "now" })
                    .WithExample(new[] { "now", "-u" })
                    .WithExample(new[] { "now", "-f \"yyyy-MM-dd HH:mm\" -u" });
                configurator.AddCommand<GetPublicHolidaysCommand>("get-public-holidays");
                configurator.SetInterceptor(new WaitForDebuggerInterceptor());

                // https://spectreconsole.net/cli/exceptions
                configurator.PropagateExceptions();
            });

            try
            {
                await app.RunAsync(Environment.GetCommandLineArgs().Skip(1).ToArray());
                Environment.ExitCode = 0;
            }
            catch (CommandParseException ex)
            {
                AnsiConsole.MarkupLine("[bold red]{0}[/]", ex.Message);
                Environment.ExitCode = -1;
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                Environment.ExitCode = -99;
            }

            logger.LogTrace("Stopping");
            TaskCompletionSource.SetResult(true);

            // we need to call StopApplication so IHostedService lifetime finishes when our console application is done with its work/execution.
            appLifetime.StopApplication();
        }
    }
}