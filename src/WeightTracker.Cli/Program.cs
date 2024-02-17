using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WeightTracker.Cli;
using WeightTracker.Cli.Authentication;
using WeightTracker.Cli.Extensions;
using WeightTracker.Cli.Infrastructure;
using WeightTracker.Client.Configuration;

var app = new Application();

app.BuildConfiguration("ENV");

app.ConfigureServices((services, configuration) =>
{
    var baseUrl = configuration.GetSection("Api:BaseUrl").Value;

    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        throw new InvalidOperationException("Base URL must be configured.");
    }

    services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.Position));

    services.AddScoped<IAuthService, AuthService>();

    services.AddApiClient(baseUrl);
});

app.ConfigureCli(config =>
{
    var commands = Assembly.GetExecutingAssembly()
        .GetCommandTypesFromAssembly()
        .OrderBy(t => t.Name);

    foreach (var command in commands)
    {
        config.RegisterCommands(
            command,
            command.GetCommandName(Constants.CommandPostfix));
    }

    config.SetExceptionHandler(ex =>
    {
        // TODO: update this
        // AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    });
});

return await app.RunAsync(args);
