using Azure.Data.Tables;
using Mapster;
using WeightTracker.Domain.Common.Extensions;
using WeightTracker.Domain.Weight.Models;
using WeightTracker.Domain.Weight.Services;

namespace WeightTracker.Api.Infrastructure.Data;

/// <inheritdoc />
/// <remarks>
/// This class is used to interact with the Azure Table Storage to store and retrieve weight data.
/// </remarks>
/// <seealso cref="IWeightDataService" />
internal sealed class WeightDataService(TableServiceClient tableServiceClient) : IWeightDataService
{
    private const string TableName = "WeightData";

    /// <inheritdoc />
    public async Task AddAsync(WeightData weightData)
    {
        var tableClient = await GetTableClientAsync();
        var entity = weightData.Adapt<WeightDataEntity>();
        await tableClient.AddEntityAsync(entity);
    }

    /// <inheritdoc />
    public async Task<WeightDataGroup> GetAsync(WeightDataFilter weightDataFilter)
    {
        var tableClient = await GetTableClientAsync();
        var (userId, dateFrom, dateTo) = weightDataFilter;

        var from = (dateFrom ?? DateOnly.MinValue).ToFormattedString();
        var to = (dateTo ?? DateOnly.MaxValue).ToFormattedString();

        var filter = $"PartitionKey eq '{userId}' and RowKey ge '{from}' and RowKey le '{to}'";
        var result = tableClient.Query<WeightDataEntity>(filter).ToList(); // TODO: check if this is correct

        var data = result.Adapt<IEnumerable<WeightData>>();
        return WeightDataGroup.Create(userId, data);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(WeightData weightData)
    {
        var tableClient = await GetTableClientAsync();
        var entity = weightData.Adapt<WeightDataEntity>();
        await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
    }

    /// <inheritdoc />
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
