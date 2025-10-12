using System.Threading.Tasks;
using WeightTracker.Core.Models;

namespace WeightTracker.Core;

public interface IDataRepository
{
    Task AddAsync(WeightData weightData);

    Task<WeightDataGroup> GetAsync(WeightDataFilter filter);

    Task UpdateAsync(WeightData weightData);

    Task DeleteAsync(string userId, DateOnly date);
}
