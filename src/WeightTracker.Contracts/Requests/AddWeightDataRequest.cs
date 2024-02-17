using System.Text.Json.Serialization;

namespace WeightTracker.Contracts.Requests;

public sealed class AddWeightDataRequest
{
    [JsonPropertyName("weight")]
    public decimal Weight { get; init; }

    [JsonPropertyName("date")]
    public string? Date { get; init; }
}
