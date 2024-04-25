using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;

namespace WeightTracker.Client;

/// <summary>
/// Represents the API client.
/// </summary>
public interface IApiClient
{
    /// <summary>
    /// Gets the weight data asynchronously.
    /// </summary>
    /// <param name="queryParams">The query parameters for filtering the weight data.</param>
    /// <param name="accessToken">The access token for authorization.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing an asynchronous operation that returns the weight data group DTO.</returns>
    Task<WeightDataGroupDto> GetWeightDataAsync(GetWeightDataQueryParams queryParams, string accessToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds the weight data asynchronously.
    /// </summary>
    /// <param name="request">The request with the weight data to add.</param>
    /// <param name="accessToken">The access token for authorization.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing an asynchronous operation.</returns>
    Task AddWeightDataAsync(AddWeightDataRequest request, string accessToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the weight data asynchronously.
    /// </summary>
    /// <param name="date">The date of the weight data to update.</param>
    /// <param name="request">The request with the weight data to update.</param>
    /// <param name="accessToken">The access token for authorization.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing an asynchronous operation.</returns>
    Task UpdateWeightDataAsync(string date, UpdateWeightDataRequest request, string accessToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the weight data asynchronously.
    /// </summary>
    /// <param name="date">The date of the weight data to delete.</param>
    /// <param name="accessToken">The access token for authorization.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing an asynchronous operation.</returns>
    Task DeleteWeightDataAsync(string date, string accessToken, CancellationToken cancellationToken = default);
}
