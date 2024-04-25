using JetBrains.Annotations;
using Mapster;
using WeightTracker.Api.Entities;
using WeightTracker.Api.Extensions;
using WeightTracker.Api.Models;
using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;

namespace WeightTracker.Api;

/// <summary>
/// Represents the mapping's configuration.
/// </summary>
/// <remarks>
/// This class is used by <see cref="TypeAdapterConfig"/> to register the mappings.
/// </remarks>
/// <example>
/// This example shows how to register the mappings.
/// <code>
/// TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
/// </code>
/// And this is how to use it:
/// <code>
/// var weightData = new WeightData();
/// var entity = weightData.Adapt&lt;WeightDataEntity&gt;();
/// </code>
/// </example>
/// <seealso cref="IRegister" />
[UsedImplicitly]
internal sealed class Mappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.MapToConstructor(true);

        config.ForType<WeightData, WeightDataEntity>()
            .Map(dest => dest.PartitionKey, src => src.UserId)
            .Map(dest => dest.RowKey, src => src.Date.ToFormattedString());

        config.ForType<WeightDataEntity, WeightData>()
            .Map(dest => dest.UserId, src => src.PartitionKey)
            .Map(dest => dest.Date, src => DateOnly.Parse(src.RowKey));

        config.ForType<(string UserId, AddWeightDataRequest Request), WeightData>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.Date, src =>
                string.IsNullOrEmpty(src.Request.Date)
                    ? DateOnly.FromDateTime(DateTime.Today.Date)
                    : DateOnly.Parse(src.Request.Date));

        config.ForType<(string UserId, GetWeightDataQueryParams Filter), DataFilter>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Filter);

        config.ForType<(string UserId, string Date, UpdateWeightDataRequest Request), WeightData>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest, src => src.Request);

        config.ForType<WeightData, WeightDataListItemDto>()
            .Map(dest => dest.Date, src => src.Date.ToFormattedString());
    }
}
