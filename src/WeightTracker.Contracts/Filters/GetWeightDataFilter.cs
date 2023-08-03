namespace WeightTracker.Contracts.Filters;

public sealed class GetWeightDataFilter
{
    public string? DateFrom { get; init; }

    public string? DateTo { get; init; }
}
