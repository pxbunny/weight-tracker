using System.Text.Json.Serialization;

namespace WeightTracker.Contracts.Requests;

/// <summary>
/// Represents a request to update weight data.
/// </summary>
public sealed class UpdateWeightDataRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateWeightDataRequest"/> class.
    /// </summary>
    /// <remarks>
    /// The constructor without parameters is required for deserialization.
    /// </remarks>
    public UpdateWeightDataRequest()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateWeightDataRequest"/> class.
    /// </summary>
    /// <param name="weight">The new weight to update.</param>
    public UpdateWeightDataRequest(decimal weight)
    {
        Weight = weight;
    }

    /// <summary>
    /// Gets or initializes the new weight.
    /// </summary>
    /// <value>
    /// The new weight to update.
    /// </value>
    [JsonPropertyName("weight")]
    public decimal Weight { get; init; }
}
