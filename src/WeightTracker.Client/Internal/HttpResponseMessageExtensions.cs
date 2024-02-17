using System.Text.Json;

namespace WeightTracker.Client.Internal;

internal static class HttpResponseMessageExtensions
{
    public static async Task<T> ReadContentAsAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken)
    {
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var data = JsonSerializer.Deserialize<T>(content);

        if (data is null)
        {
            throw new InvalidOperationException("Failed to deserialize response.");
        }

        return data;
    }
}
