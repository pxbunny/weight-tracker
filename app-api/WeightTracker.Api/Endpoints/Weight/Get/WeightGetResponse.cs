namespace WeightTracker.Api.Endpoints.Weight.Get;

public sealed class WeightGetResponse
{
    public string UserId { get; init; } = null!;

    public decimal? Avg { get; init; }

    public decimal? Max { get; init; }

    public decimal? Min { get; init; }

    public IEnumerable<WeightDataListItem> Data { get; init; } = null!;
}

public sealed record WeightDataListItem(string Date, decimal Weight);
