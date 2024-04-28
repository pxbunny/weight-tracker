using Microsoft.Extensions.DependencyInjection;
using WeightTracker.ApiClient.Client;

namespace WeightTracker.ApiClient;

/// <summary>
/// Contains the extension method to add the API client to the service collection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the API client to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="baseUrl">The base URL of the API.</param>
    /// <returns>The service collection.</returns>
    /// <exception cref="ArgumentException">Thrown when the base URL is null or whitespace.</exception>
    public static IServiceCollection AddApiClient(this IServiceCollection services, string? baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(baseUrl));
        }

        services.AddHttpClient<IApiClient, Client.ApiClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });

        return services;
    }
}
