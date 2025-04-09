namespace WeightTracker.Api.Endpoints.Weight.Get;

public sealed class GetWeightRequest
{
    [FromQuery]
    public GetWeightQueryParams QueryParams { get; init; } = null!;
}

public sealed record GetWeightQueryParams(string? DateFrom, string? DateTo);
