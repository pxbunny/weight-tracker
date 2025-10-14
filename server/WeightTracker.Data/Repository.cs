using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WeightTracker.Data;

internal sealed class Repository(TableServiceClient tableServiceClient) : IDataRepository
{
    private const string TableName = "WeightData";

    public async Task AddAsync(WeightData weightData, CancellationToken ct)
    {
        var tableClient = await GetTableClientAsync(ct);
        var entity = weightData.ToEntity();
        await tableClient.AddEntityAsync(entity, ct);
    }

    public async Task<WeightDataGroup> GetAsync(WeightDataFilter weightDataFilter, CancellationToken ct)
    {
        var tableClient = await GetTableClientAsync(ct);
        var (userId, dateFrom, dateTo) = weightDataFilter;

        var from = (dateFrom ?? DateOnly.MinValue).ToDomainDateString();
        var to = (dateTo ?? DateOnly.MaxValue).ToDomainDateString();

        var filter = $"PartitionKey eq '{userId}' and RowKey ge '{from}' and RowKey le '{to}'";
        var result = tableClient.Query<Entity>(filter, cancellationToken: ct).ToList();

        var data = result.Select(e => e.ToDomain());
        var dataGroup = WeightDataGroup.Create(userId, data);

        return dataGroup;
    }

    public async Task UpdateAsync(WeightData weightData, CancellationToken ct)
    {
        var tableClient = await GetTableClientAsync(ct);
        var entity = weightData.ToEntity();
        await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace, ct);
    }

    public async Task DeleteAsync(string userId, DateOnly date, CancellationToken ct)
    {
        var tableClient = await GetTableClientAsync(ct);
        await tableClient.DeleteEntityAsync(userId, date.ToDomainDateString(), cancellationToken: ct);
    }

    private async Task<TableClient> GetTableClientAsync(CancellationToken ct)
    {
        var tableClient = tableServiceClient.GetTableClient(tableName: TableName);
        await tableClient.CreateIfNotExistsAsync(ct);
        return tableClient;
    }
}
