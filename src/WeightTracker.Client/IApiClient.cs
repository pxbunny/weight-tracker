using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.QueryParams;

namespace WeightTracker.Client;

public interface IApiClient
{
    Task<WeightDataGroupDto> GetWeightDataAsync(GetWeightDataQueryParams queryParams, CancellationToken cancellationToken = default);
}
