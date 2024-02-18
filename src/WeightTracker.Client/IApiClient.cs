using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;

namespace WeightTracker.Client;

public interface IApiClient
{
    Task<WeightDataGroupDto> GetWeightDataAsync(GetWeightDataQueryParams queryParams, string accessToken, CancellationToken cancellationToken = default);

    Task AddWeightDataAsync(AddWeightDataRequest request, string accessToken, CancellationToken cancellationToken = default);

    Task UpdateWeightDataAsync(string date, UpdateWeightDataRequest request, string accessToken, CancellationToken cancellationToken = default);

    Task DeleteWeightDataAsync(string date, string accessToken, CancellationToken cancellationToken = default);
}
