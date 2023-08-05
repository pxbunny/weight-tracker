using System.Text.Json.Serialization;

namespace WeightTracker.Contracts.DTOs;

public sealed class WeightDataListItemDto
{
    [JsonPropertyName("date")]
    public string Date { get; init; } = null!;

    [JsonPropertyName("weight")]
    public decimal Weight { get; init; }
}
