using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeightTracker.Cli.Global;

namespace WeightTracker.Cli;

public class Application
{
    private readonly CommandApp _app;
    private readonly IServiceCollection _services;
    
    private IConfiguration? _configuration;

    public Application()
    {
        _services = new ServiceCollection();
        var registrar = new Registrar.ServiceRegistrar(_services);
        _app = new CommandApp(registrar);
    }

    public void BuildConfiguration(string envVariableName)
    {
        var environment = Environment.GetEnvironmentVariable(envVariableName);
        var configBuilder = new ConfigurationBuilder().AddJsonFile($"{Constants.AppSettingsName}.json");

        if (!string.IsNullOrEmpty(environment))
        {
            configBuilder.AddJsonFile($"{Constants.AppSettingsName}.{environment}.json");
        }

        _configuration = configBuilder
            .AddEnvironmentVariables()
            .Build();
    }

    public void ConfigureServices(Action<IServiceCollection, IConfiguration> config)
    {
        if (_configuration == null)
        {
            throw new InvalidOperationException("Configuration must be built before services can be configured.");
        }

        config(_services, _configuration);
    }

    public void ConfigureCli(Action<IConfigurator> configuration) => _app.Configure(configuration);

    public Task<int> RunAsync(IEnumerable<string> args) => _app.RunAsync(args);
}
