namespace WeightTracker.Domain.Weight.Models;

/// <summary>
/// Represents the weight data model.
/// </summary>
public sealed class WeightData
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    /// <value>The user ID.</value>
    public required string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>The date.</value>
    public required DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the weight value.
    /// </summary>
    /// <value>The weight value.</value>
    public required decimal Weight { get; set; }
}
