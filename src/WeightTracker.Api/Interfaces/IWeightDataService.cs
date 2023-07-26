using WeightTracker.Api.Models;

namespace WeightTracker.Api.Interfaces;

public interface IWeightDataService
{
    Task AddAsync(WeightData weightData);
    
    // Task<IEnumerable<WeightData>> GetAsync(string userId);
    
    Task<WeightData> GetAsync(string userId, DateOnly date);
    
    Task UpdateAsync(WeightData weightData);
    
    // Task DeleteAsync(string userId, DateOnly date);
}
