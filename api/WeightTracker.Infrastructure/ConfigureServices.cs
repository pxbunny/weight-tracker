using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeightTracker.Application.Interfaces;
using WeightTracker.Infrastructure.Data;

namespace WeightTracker.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var storageConnectionString = configuration.GetSection("AzureWebJobsStorage").Value;
        
        services.AddScoped(_ => new TableServiceClient(storageConnectionString));
        services.AddScoped<IWeightDataRepository, WeightDataRepository>();

        return services;
    }
}
