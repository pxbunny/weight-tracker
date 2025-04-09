namespace WeightTracker.Api.Endpoints.Weight.Get;

public sealed class GetWeightResponse
{
    public string UserId { get; init; } = null!;

    public decimal? AverageWeight { get; init; }

    public decimal? MaxWeight { get; init; }

    public decimal? MinWeight { get; init; }

    public IEnumerable<WeightDataListItem> Data { get; init; } = null!;
}

public sealed record WeightDataListItem(string Date, decimal Weight);
