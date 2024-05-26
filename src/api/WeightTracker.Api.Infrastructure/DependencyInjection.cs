using Mapster;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeightTracker.Api.Infrastructure.Data;
using WeightTracker.Api.Infrastructure.Http;
using WeightTracker.Domain.Common.Interfaces;
using WeightTracker.Domain.Weight.Services;

namespace WeightTracker.Api.Infrastructure;

public static class DependencyInjection
{
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
