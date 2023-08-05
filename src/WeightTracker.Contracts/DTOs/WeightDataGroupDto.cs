using System.Text.Json.Serialization;

namespace WeightTracker.Contracts.DTOs;

public sealed class WeightDataGroupDto
{
    [JsonPropertyName("user_id")]
    public string UserId { get; init; } = null!;

    [JsonPropertyName("average_weight")]
    public decimal? AverageWeight { get; init; }
    
    [JsonPropertyName("max_weight")]
    public decimal? MaxWeight { get; init; }
    
    [JsonPropertyName("min_weight")]
    public decimal? MinWeight { get; init; }

    [JsonPropertyName("data")]
    public IEnumerable<WeightDataListItemDto> Data { get; init; } = null!;
}
