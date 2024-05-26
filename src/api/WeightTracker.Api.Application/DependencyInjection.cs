using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace WeightTracker.Api.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        TypeAdapterConfig.GlobalSettings.Scan(assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        return services;
    }
}
