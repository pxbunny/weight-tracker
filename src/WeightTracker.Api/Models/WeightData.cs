namespace WeightTracker.Api.Models;

public class WeightData
{
    public string UserId { get; set; }
    
    public DateOnly Date { get; set; }
    
    public double Weight { get; set; }
}
