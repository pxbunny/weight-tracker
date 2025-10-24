namespace WeightTracker.Api.Endpoints.Weight.Get;

internal sealed class WeightGetResponse
{
    public string UserId { get; init; } = null!;

    public decimal? Avg { get; init; }

    public decimal? Max { get; init; }

    public decimal? Min { get; init; }

    public IEnumerable<WeightDataListItem> Data { get; init; } = null!;
}

internal sealed record WeightDataListItem(string Date, decimal Weight);
