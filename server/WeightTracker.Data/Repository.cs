using System.Linq;
using System.Threading.Tasks;

namespace WeightTracker.Data;

internal sealed class Repository(TableServiceClient tableServiceClient) : IDataRepository
{
    private const string TableName = "WeightData";

    public async Task AddAsync(WeightData weightData)
    {
        var tableClient = await GetTableClientAsync();
        var entity = weightData.ToEntity();
        await tableClient.AddEntityAsync(entity);
    }

    public async Task<WeightDataGroup> GetAsync(WeightDataFilter weightDataFilter)
    {
        var tableClient = await GetTableClientAsync();
        var (userId, dateFrom, dateTo) = weightDataFilter;

        var from = (dateFrom ?? DateOnly.MinValue).ToFormattedString();
        var to = (dateTo ?? DateOnly.MaxValue).ToFormattedString();

        var filter = $"PartitionKey eq '{userId}' and RowKey ge '{from}' and RowKey le '{to}'";
        var result = tableClient.Query<Entity>(filter).ToList();

        var data = result.Select(e => e.ToDomain());
        var dataGroup = WeightDataGroup.Create(userId, data);

        return dataGroup;
    }

    public async Task UpdateAsync(WeightData weightData)
    {
        var tableClient = await GetTableClientAsync();
        var entity = weightData.ToEntity();
        await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
    }

    public async Task DeleteAsync(string userId, DateOnly date)
    {
        var tableClient = await GetTableClientAsync();
        await tableClient.DeleteEntityAsync(userId, date.ToFormattedString());
    }

    private async Task<TableClient> GetTableClientAsync()
    {
        var tableClient = tableServiceClient.GetTableClient(tableName: TableName);
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
}
