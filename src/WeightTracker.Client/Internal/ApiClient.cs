using WeightTracker.Contracts;
using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.QueryParams;

namespace WeightTracker.Client.Internal;

internal sealed class ApiClient : IApiClient
{
    private readonly HttpClient _client;

    public ApiClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<WeightDataGroupDto> GetWeightDataAsync(GetWeightDataQueryParams queryParams, CancellationToken cancellationToken = default)
    {
        var queryString = queryParams.BuildQueryString();
        var uri = $"{Routes.GetWeightData}?{queryString}";
        var response = await _client.GetAsync(uri, cancellationToken);
        return await response.ReadContentAsAsync<WeightDataGroupDto>(cancellationToken);
    }
}
