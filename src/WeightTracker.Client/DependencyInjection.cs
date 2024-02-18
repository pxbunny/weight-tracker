using Microsoft.Extensions.DependencyInjection;
using WeightTracker.Client.Client;

namespace WeightTracker.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddApiClient(this IServiceCollection services, string baseUrl)
    {
        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("Base URL must be configured.");
            }

            client.BaseAddress = new Uri(baseUrl);
        });

        return services;
    }
}
