using WeightTracker.Api.Models;

namespace WeightTracker.Api.Interfaces;

public interface IWeightDataService
{
    Task AddAsync(WeightData weightData);

    Task<WeightDataGroup> GetAsync(DataFilter filter);

    Task UpdateAsync(WeightData weightData);

    Task DeleteAsync(string userId, DateOnly date);
}
