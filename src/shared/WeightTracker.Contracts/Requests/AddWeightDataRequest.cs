using System.Text.Json.Serialization;

namespace WeightTracker.Contracts.Requests;

/// <summary>
/// Represents a request to add weight data.
/// </summary>
public sealed class AddWeightDataRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddWeightDataRequest"/> class.
    /// </summary>
    /// <remarks>
    /// The constructor without parameters is required for deserialization.
    /// </remarks>
    public AddWeightDataRequest()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddWeightDataRequest"/> class.
    /// </summary>
    /// <param name="weight">The weight to add for the specified date.</param>
    /// <param name="date">The date of the weight data.</param>
    public AddWeightDataRequest(decimal weight, string? date)
    {
        Weight = weight;
        Date = date;
    }

    /// <summary>
    /// Gets or initializes the weight.
    /// </summary>
    /// <value>
    /// The weight to add for the specified date.
    /// </value>
    [JsonPropertyName("weight")]
    public decimal Weight { get; init; }

    /// <summary>
    /// Gets or initializes the date of the weight data.
    /// </summary>
    /// <value>
    /// The date when the weight data was recorded.
    /// </value>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>
    /// Deconstructs the request into its components.
    /// </summary>
    /// <param name="weight">The weight.</param>
    /// <param name="date">The date as a string.</param>
    public void Deconstruct(
        out decimal weight,
        out string? date)
    {
        weight = Weight;
        date = Date;
    }
}
