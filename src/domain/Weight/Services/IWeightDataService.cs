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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(WeightData weightData);

    /// <summary>
    /// Gets the weight data asynchronously.
    /// </summary>
    /// <param name="filter">The <see cref="WeightDataFilter"/> used to filter the weight data.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains the <see cref="WeightDataGroup"/>.
    /// </returns>
    Task<WeightDataGroup> GetAsync(WeightDataFilter filter);

    /// <summary>
    /// Updates the weight data asynchronously.
    /// </summary>
    /// <param name="weightData">The weight data.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(WeightData weightData);

    /// <summary>
    /// Deletes the weight data asynchronously.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="date">The date of the weight data entry.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(string userId, DateOnly date);
}
