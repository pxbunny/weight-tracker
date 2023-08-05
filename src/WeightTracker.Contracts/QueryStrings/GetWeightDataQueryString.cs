namespace WeightTracker.Contracts.QueryStrings;

public sealed class GetWeightDataQueryString
{
    public string? DateFrom { get; init; }
    
    public string? DateTo { get; init; }
}
