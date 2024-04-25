using System.Text.Json.Serialization;

namespace WeightTracker.Contracts.DTOs;

/// <summary>
/// Represents a list item DTO of weight data.
/// </summary>
public sealed class WeightDataListItemDto
{
    /// <summary>
    /// Gets or initializes the date of the weight data.
    /// </summary>
    /// <value>
    /// The date when the weight data was recorded.
    /// </value>
    [JsonPropertyName("date")]
    public string Date { get; init; } = null!;

    /// <summary>
    /// Gets or initializes the weight.
    /// </summary>
    /// <value>
    /// The weight recorded on the specified date.
    /// </value>
    [JsonPropertyName("weight")]
    public decimal Weight { get; init; }
}
