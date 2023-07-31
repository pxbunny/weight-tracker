using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WeightTracker.Cli;
using WeightTracker.Cli.Configuration;
using WeightTracker.Cli.Extensions;
using WeightTracker.Cli.Global;
using WeightTracker.Cli.Services;

var app = new Application();
app.BuildConfiguration("ENV");

app.ConfigureServices((services, configuration) =>
{
    services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.Position));
    
    services.AddScoped<IAuthService, AuthService>();

    services.AddHttpClient<IApiService, ApiService>(client =>
    {
        var baseUrl = configuration.GetSection("Api:BaseUrl").Value;
        
        if (string.IsNullOrEmpty(baseUrl))
        {
            throw new InvalidOperationException("Base URL must be configured.");
        }
        
        client.BaseAddress = new Uri(baseUrl);
    });
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
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    });
});

return await app.RunAsync(args);
