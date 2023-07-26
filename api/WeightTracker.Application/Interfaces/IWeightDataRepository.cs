using WeightTracker.Application.Models;

namespace WeightTracker.Application.Interfaces;

public interface IWeightDataRepository
{
    Task AddAsync(WeightData weightData);
    
    // Task<IEnumerable<WeightData>> GetAsync(string userId);
    
    Task<WeightData> GetAsync(string userId, DateOnly date);
    
    Task UpdateAsync(WeightData weightData);
    
    // Task DeleteAsync(string userId, DateOnly date);
}
