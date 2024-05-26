using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WeightTracker.Api.Application.Common.Behaviors;

namespace WeightTracker.Api.Application;

/// <summary>
/// Contains the dependency injection configuration for the application layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the application services to the specified service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        TypeAdapterConfig.GlobalSettings.Scan(assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        });

        return services;
    }
}
