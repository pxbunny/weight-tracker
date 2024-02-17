namespace WeightTracker.Api.Models;

public class WeightData
{
    public string UserId { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public decimal Weight { get; set; }
}
