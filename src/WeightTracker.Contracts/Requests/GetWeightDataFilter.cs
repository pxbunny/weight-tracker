namespace WeightTracker.Contracts.Requests;

public sealed class GetWeightDataFilter
{
    public string? DateFrom { get; init; }

    public string? DateTo { get; init; }
}
