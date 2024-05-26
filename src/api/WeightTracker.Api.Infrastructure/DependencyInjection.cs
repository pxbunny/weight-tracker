using Mapster;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeightTracker.Api.Application.Common.Interfaces;
using WeightTracker.Api.Infrastructure.Data;
using WeightTracker.Api.Infrastructure.Http;
using WeightTracker.Domain.Weight.Services;

namespace WeightTracker.Api.Infrastructure;

/// <summary>
/// Represents the dependency injection for the infrastructure layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the infrastructure services to the specified <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        TypeAdapterConfig.GlobalSettings.Scan(assembly);

        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddTableServiceClient(configuration["AzureWebJobsStorage"]);
        });

        services.AddScoped<IWeightDataService, WeightDataService>();
        services.AddScoped<IUser, CurrentUserService>();

        return services;
    }
}
