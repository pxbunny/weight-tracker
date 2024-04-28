namespace WeightTracker.WebApi.Models;

/// <summary>
/// Represents the weight data model.
/// </summary>
internal sealed class WeightData
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    /// <value>The user ID.</value>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>The date.</value>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the weight value.
    /// </summary>
    /// <value>The weight value.</value>
    public decimal Weight { get; set; }
}
