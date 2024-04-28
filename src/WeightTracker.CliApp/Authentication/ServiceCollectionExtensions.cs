using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WeightTracker.CliApp.Authentication;

/// <summary>
/// Contains extension methods for the <see cref="IServiceCollection"/>.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds authentication services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.Position));
        services.AddScoped<AuthService>();
        return services;
    }
}
