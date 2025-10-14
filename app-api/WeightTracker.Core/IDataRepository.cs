using System.Threading;
using System.Threading.Tasks;
using WeightTracker.Core.Models;

namespace WeightTracker.Core;

public interface IDataRepository
{
    Task AddAsync(WeightData weightData, CancellationToken ct);

    Task<WeightDataGroup> GetAsync(WeightDataFilter filter, CancellationToken ct);

    Task UpdateAsync(WeightData weightData, CancellationToken ct);

    Task DeleteAsync(string userId, DateOnly date, CancellationToken ct);
}
