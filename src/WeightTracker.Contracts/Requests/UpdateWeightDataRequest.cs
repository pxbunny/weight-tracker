using System.Text.Json.Serialization;

namespace WeightTracker.Contracts.Requests;

public sealed class UpdateWeightDataRequest
{
    [JsonPropertyName("weight")]
    public decimal Weight { get; init; }
}
