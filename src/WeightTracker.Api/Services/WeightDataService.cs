using Azure.Data.Tables;
using Mapster;
using WeightTracker.Api.Extensions;
using WeightTracker.Api.Interfaces;
using WeightTracker.Api.Models;

namespace WeightTracker.Api.Services;

internal sealed class WeightDataService : IWeightDataService
{
    private const string TableName = "WeightData";
    
    private readonly TableServiceClient _tableServiceClient;
    
    public WeightDataService(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
    }

    public async Task AddAsync(WeightData weightData)
    {
        var tableClient = await GetTableClientAsync();
        var entity = weightData.Adapt<WeightDataEntity>();
        await tableClient.AddEntityAsync(entity);
    }

    public async Task<IEnumerable<WeightData>> GetAsync(WeightDataFilter dataFilter)
    {
        var tableClient = await GetTableClientAsync();
        var (userId, dateFrom, dateTo) = dataFilter;
        
        var from = (dateFrom ?? DateOnly.MinValue).ToFormattedString();
        var to = (dateTo ?? DateOnly.MaxValue).ToFormattedString();

        var filter = $"PartitionKey eq '{userId}' and RowKey ge '{from}' and RowKey le '{to}'";
        var result = tableClient.Query<WeightDataEntity>(filter).ToList();
        
        return result.Adapt<IEnumerable<WeightData>>();
    }

    public async Task UpdateAsync(WeightData weightData)
    {
        var tableClient = await GetTableClientAsync();
        var entity = weightData.Adapt<WeightDataEntity>();
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
