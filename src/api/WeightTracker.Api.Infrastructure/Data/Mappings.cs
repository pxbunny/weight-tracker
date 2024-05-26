using JetBrains.Annotations;
using Mapster;
using WeightTracker.Domain.Common.Extensions;
using WeightTracker.Domain.Weight.Models;

namespace WeightTracker.Api.Infrastructure.Data;

/// <summary>
/// Represents the mapping's configuration.
/// </summary>
/// <remarks>
/// This class is used by <see cref="TypeAdapterConfig"/> to register the mappings.
/// Maps only the storage entities to the domain models and vice versa.
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
    /// <summary>
    /// Registers the mappings using the specified configuration.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <seealso cref="TypeAdapterConfig"/>
    public void Register(TypeAdapterConfig config)
    {
        config.Default.MapToConstructor(true);

        config.ForType<WeightData, WeightDataEntity>()
            .Map(dest => dest.PartitionKey, src => src.UserId)
            .Map(dest => dest.RowKey, src => src.Date.ToFormattedString());

        config.ForType<WeightDataEntity, WeightData>()
            .Map(dest => dest.UserId, src => src.PartitionKey)
            .Map(dest => dest.Date, src => DateOnly.Parse(src.RowKey));
    }
}
