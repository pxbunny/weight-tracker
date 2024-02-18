using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WeightTracker.Cli.Authentication;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.Position));
        services.AddScoped<AuthService>();
        return services;
    }
}
