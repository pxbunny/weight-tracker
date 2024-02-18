namespace WeightTracker.Api.Models;

internal sealed class WeightData
{
    public string UserId { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public decimal Weight { get; set; }
}
