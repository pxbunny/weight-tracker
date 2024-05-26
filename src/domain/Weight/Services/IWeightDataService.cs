using WeightTracker.Domain.Common.Response;
using WeightTracker.Domain.Weight.Models;

namespace WeightTracker.Domain.Weight.Services;

/// <summary>
/// Represents the weight data service.
/// </summary>
public interface IWeightDataService
{
    /// <summary>
    /// Adds the weight data asynchronously.
    /// </summary>
    /// <param name="weightData">The weight data.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains the <see cref="Response"/>.
    /// </returns>
    Task<IResponse> AddAsync(WeightData weightData);

    /// <summary>
    /// Gets the weight data asynchronously.
    /// </summary>
    /// <param name="filter">The <see cref="WeightDataFilter"/> used to filter the weight data.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains the <see cref="Response"/> with the <see cref="WeightDataGroup"/>.
    /// </returns>
    Task<IResponse<WeightDataGroup>> GetAsync(WeightDataFilter filter);

    /// <summary>
    /// Updates the weight data asynchronously.
    /// </summary>
    /// <param name="weightData">The weight data.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains the <see cref="Response"/>.
    /// </returns>
    Task<IResponse> UpdateAsync(WeightData weightData);

    /// <summary>
    /// Deletes the weight data asynchronously.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="date">The date of the weight data entry.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains the <see cref="Response"/>.
    /// </returns>
    Task<IResponse> DeleteAsync(string userId, DateOnly date);
}
