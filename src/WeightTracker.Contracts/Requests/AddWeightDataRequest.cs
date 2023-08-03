namespace WeightTracker.Contracts.Requests;

public sealed class AddWeightDataRequest
{
    public decimal Weight { get; init; }
    
    public string? Date { get; init; }
}
