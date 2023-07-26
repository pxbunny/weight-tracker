using Azure.Data.Tables;
using WeightTracker.Application.Interfaces;
using WeightTracker.Application.Models;

namespace WeightTracker.Infrastructure.Data;

internal sealed class WeightDataRepository : IWeightDataRepository
{
    private const string TableName = "WeightData";
    
    private readonly TableServiceClient _tableServiceClient;
    
    public WeightDataRepository(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
    }

    public async Task AddAsync(WeightData weightData)
    {
        var tableClient = await GetTableClientAsync();
        // var entity = weightData.Adapt<WeightDataEntity>();
        var entity = new WeightDataEntity
        {
            PartitionKey = weightData.UserId,
            RowKey = weightData.Date.ToString("yyyy.MM.dd"),
            Weight = weightData.Weight
        };
        await tableClient.AddEntityAsync(entity);
    }

    // public async Task<IEnumerable<WeightData>> GetAsync(string userId)
    // {
    //     var tableClient = await GetTableClientAsync();
    //     var x = tableClient.Query<WeightDataEntity>(a => a.RowKey == userId);
    //     
    // }

    public async Task<WeightData> GetAsync(string userId, DateOnly date)
    {
        var tableClient = await GetTableClientAsync();
        var result = tableClient
            .Query<WeightDataEntity>(w => w.PartitionKey == userId && w.RowKey == date.ToString("yyyy.MM.dd"))
            .FirstOrDefault();
        // return result.Adapt<WeightData>();
        return new WeightData
        {
            UserId = result.PartitionKey,
            Date = DateOnly.Parse(result.RowKey),
            Weight = result.Weight
        };
    }

    public async Task UpdateAsync(WeightData weightData)
    {
        var tableClient = await GetTableClientAsync();
        // var entity = weightData.Adapt<WeightDataEntity>();
        var entity = new WeightDataEntity
        {
            PartitionKey = weightData.UserId,
            RowKey = weightData.Date.ToString("yyyy.MM.dd"),
            Weight = weightData.Weight
        };
        await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
    }

    // public async Task DeleteAsync(string userId, DateOnly date)
    // {
    //     var tableClient = await GetTableClientAsync();
    //     await tableClient.DeleteEntityAsync(userId, date.ToString());
    // }
    
    private async Task<TableClient> GetTableClientAsync()
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName: TableName);
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
}
