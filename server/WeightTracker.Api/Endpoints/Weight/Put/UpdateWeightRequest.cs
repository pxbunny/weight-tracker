namespace WeightTracker.Api.Endpoints.Weight.Put;

public sealed class UpdateWeightRequest
{
    public string Date { get; init; } = null!;

    public decimal Weight { get; init; }
}
