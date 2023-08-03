namespace WeightTracker.Contracts.Responses;

public sealed class GetWeightDataResponse
{
    public decimal Weight { get; init; }
    
    public string Date { get; init; }
}
