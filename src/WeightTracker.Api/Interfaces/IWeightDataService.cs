using WeightTracker.Api.Models;

namespace WeightTracker.Api.Interfaces;

public interface IWeightDataService
{
    Task AddAsync(WeightData weightData);
    
    // Task<IEnumerable<WeightData>> GetAsync(string userId);
    
    Task<IEnumerable<WeightData>> GetAsync(WeightDataFilter filter);
    
    Task UpdateAsync(WeightData weightData);
    
    // Task DeleteAsync(string userId, DateOnly date);
}
