using System.Text.Json.Serialization;

namespace WeightTracker.Contracts.Requests;

public sealed class UpdateWeightDataRequest
{
    public UpdateWeightDataRequest()
    {
    }

    public UpdateWeightDataRequest(decimal weight)
    {
        Weight = weight;
    }

    [JsonPropertyName("weight")]
    public decimal Weight { get; init; }
}
