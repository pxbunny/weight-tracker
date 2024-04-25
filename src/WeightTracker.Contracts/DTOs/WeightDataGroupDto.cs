using System.Text.Json.Serialization;

namespace WeightTracker.Contracts.DTOs;

/// <summary>
/// Represents a DTO of a weight data group.
/// </summary>
/// <remarks>
/// The group contains the user ID, the average weight,
/// the maximum weight, the minimum weight, and the weight data list.
/// </remarks>
public sealed class WeightDataGroupDto
{
    /// <summary>
    /// Gets or initializes the user ID.
    /// </summary>
    /// <value>
    /// The user ID.
    /// </value>
    [JsonPropertyName("user_id")]
    public string UserId { get; init; } = null!;

    /// <summary>
    /// Gets or initializes the average weight.
    /// </summary>
    /// <value>
    /// The average weight for the specified period.
    /// </value>
    [JsonPropertyName("average_weight")]
    public decimal? AverageWeight { get; init; }

    /// <summary>
    /// Gets or initializes the maximum weight.
    /// </summary>
    /// <value>
    /// The maximum weight for the specified period.
    /// </value>
    [JsonPropertyName("max_weight")]
    public decimal? MaxWeight { get; init; }

    /// <summary>
    /// Gets or initializes the minimum weight.
    /// </summary>
    /// <value>
    /// The minimum weight for the specified period.
    /// </value>
    [JsonPropertyName("min_weight")]
    public decimal? MinWeight { get; init; }

    /// <summary>
    /// Gets or initializes the weight data list.
    /// </summary>
    /// <value>
    /// Full list of weight data for the specified period.
    /// </value>
    /// <seealso cref="WeightDataListItemDto"/>
    [JsonPropertyName("data")]
    public IEnumerable<WeightDataListItemDto> Data { get; init; } = null!;
}
