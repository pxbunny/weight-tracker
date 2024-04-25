using Azure.Data.Tables;
using Mapster;
using WeightTracker.Api.Entities;
using WeightTracker.Api.Extensions;
using WeightTracker.Api.Models;

namespace WeightTracker.Api.Services;

/// <summary>
/// Represents the weight data service.
/// </summary>
internal interface IWeightDataService
{
    /// <summary>
    /// Adds the weight data asynchronously.
    /// </summary>
    /// <param name="weightData">The weight data.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(WeightData weightData);

    /// <summary>
    /// Gets the weight data asynchronously.
    /// </summary>
    /// <param name="filter">The <see cref="DataFilter"/> used to filter the weight data.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains the <see cref="WeightDataGroup"/>.
    /// </returns>
    Task<WeightDataGroup> GetAsync(DataFilter filter);

    /// <summary>
    /// Updates the weight data asynchronously.
    /// </summary>
    /// <param name="weightData">The weight data.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(WeightData weightData);

    /// <summary>
    /// Deletes the weight data asynchronously.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="date">The date of the weight data entry.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(string userId, DateOnly date);
}

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
    public async Task<WeightDataGroup> GetAsync(DataFilter dataFilter)
    {
        var tableClient = await GetTableClientAsync();
        var (userId, dateFrom, dateTo) = dataFilter;

        var from = (dateFrom ?? DateOnly.MinValue).ToFormattedString();
        var to = (dateTo ?? DateOnly.MaxValue).ToFormattedString();

        var filter = $"PartitionKey eq '{userId}' and RowKey ge '{from}' and RowKey le '{to}'";
        var result = tableClient.Query<WeightDataEntity>(filter).ToList();

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
