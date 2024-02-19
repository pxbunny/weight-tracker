using System.Text.Json.Serialization;

namespace WeightTracker.Contracts.Requests;

public sealed class AddWeightDataRequest
{
    public AddWeightDataRequest()
    {
    }

    public AddWeightDataRequest(decimal weight, string? date)
    {
        Weight = weight;
        Date = date;
    }

    [JsonPropertyName("weight")]
    public decimal Weight { get; init; }

    [JsonPropertyName("date")]
    public string? Date { get; init; }
}
