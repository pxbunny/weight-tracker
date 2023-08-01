using Mapster;
using WeightTracker.Api.Controllers;
using WeightTracker.Api.Extensions;
using WeightTracker.Api.Models;

namespace WeightTracker.Api;

public sealed class MappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.MapToConstructor(true);

        config.ForType<WeightData, WeightDataEntity>()
            .Map(dest => dest.PartitionKey, src => src.UserId)
            .Map(dest => dest.RowKey, src => src.Date.ToFormattedString());

        config.ForType<WeightDataEntity, WeightData>()
            .Map(dest => dest.UserId, src => src.PartitionKey);

        config.ForType<(string UserId, AddWeightDataRequest Request), WeightData>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.Date,
                src => string.IsNullOrEmpty(src.Request.Date)
                    ? DateOnly.FromDateTime(DateTime.Today.Date)
                    : DateOnly.Parse(src.Request.Date));

        config.ForType<(string UserId, GetWeightDataFilter Filter), WeightDataFilter>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Filter);
    }
}
