using System.Text.Json;

namespace WeightTracker.ApiClient.Client;

/// <summary>
/// Contains the extension methods for <see cref="HttpResponseMessage"/>.
/// </summary>
internal static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Reads the content of an <see cref="HttpResponseMessage"/> as a specific type.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage"/>.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="T">The type to deserialize the content to.</typeparam>
    /// <returns>The deserialized content.</returns>
    /// <exception cref="JsonException">Thrown when the response content is not a valid JSON.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the response content cannot be deserialized.</exception>
    /// <example>
    /// <code>
    /// var data = await response.ReadContentAsAsync&lt;WeightDataGroupDto&gt;(cancellationToken);
    /// </code>
    /// </example>
    public static async Task<T> ReadContentAsAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken)
    {
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var data = JsonSerializer.Deserialize<T>(content);
        return data ?? throw new InvalidOperationException("Failed to deserialize response.");
    }
}
