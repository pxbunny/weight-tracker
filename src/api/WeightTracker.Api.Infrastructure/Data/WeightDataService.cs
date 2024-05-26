using Azure;
using Azure.Data.Tables;
using Mapster;
using WeightTracker.Domain.Common.Extensions;
using WeightTracker.Domain.Common.Response;
using WeightTracker.Domain.Weight.Models;
using WeightTracker.Domain.Weight.Services;
using Response = WeightTracker.Domain.Common.Response.Response;

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
    public async Task<IResponse> AddAsync(WeightData weightData)
    {
        var tableClient = await GetTableClientAsync();
        var entity = weightData.Adapt<WeightDataEntity>();

        try
        {
            await tableClient.AddEntityAsync(entity);
            return Response.Success();
        }
        catch (RequestFailedException e)
        {
            return Response.Fail(e.Message, e.Status);
        }
        catch (Exception e)
        {
            return Response.Fail(e.Message, 500);
        }
    }

    /// <inheritdoc />
    public async Task<IResponse<WeightDataGroup>> GetAsync(WeightDataFilter weightDataFilter)
    {
        var tableClient = await GetTableClientAsync();
        var (userId, dateFrom, dateTo) = weightDataFilter;

        var from = (dateFrom ?? DateOnly.MinValue).ToFormattedString();
        var to = (dateTo ?? DateOnly.MaxValue).ToFormattedString();

        var filter = $"PartitionKey eq '{userId}' and RowKey ge '{from}' and RowKey le '{to}'";

        IList<WeightDataEntity> result;

        try
        {
            result = tableClient.Query<WeightDataEntity>(filter).ToList(); // TODO: check if this is correct
        }
        catch (RequestFailedException e)
        {
            return Domain.Common.Response.Response<WeightDataGroup>.Fail(e.Message, e.Status);
        }
        catch (Exception e)
        {
            return Domain.Common.Response.Response<WeightDataGroup>.Fail(e.Message, 500);
        }

        var data = result.Adapt<IEnumerable<WeightData>>();
        var dataGroup = WeightDataGroup.Create(userId, data);
        return Domain.Common.Response.Response<WeightDataGroup>.Success(dataGroup);
    }

    /// <inheritdoc />
    public async Task<IResponse> UpdateAsync(WeightData weightData)
    {
        var tableClient = await GetTableClientAsync();
        var entity = weightData.Adapt<WeightDataEntity>();

        try
        {
            await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
            return Response.Success();
        }
        catch (RequestFailedException e)
        {
            return Response.Fail(e.Message, e.Status);
        }
        catch (Exception e)
        {
            return Response.Fail(e.Message, 500);
        }
    }

    /// <inheritdoc />
    public async Task<IResponse> DeleteAsync(string userId, DateOnly date)
    {
        var tableClient = await GetTableClientAsync();
        await tableClient.DeleteEntityAsync(userId, date.ToFormattedString());

        try
        {
            await tableClient.DeleteEntityAsync(userId, date.ToFormattedString());
            return Response.Success();
        }
        catch (RequestFailedException e)
        {
            return Response.Fail(e.Message, e.Status);
        }
        catch (Exception e)
        {
            return Response.Fail(e.Message, 500);
        }
    }

    private async Task<TableClient> GetTableClientAsync()
    {
        var tableClient = tableServiceClient.GetTableClient(tableName: TableName);
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
}
