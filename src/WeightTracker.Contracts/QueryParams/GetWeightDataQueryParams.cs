using Microsoft.AspNetCore.Mvc;

namespace WeightTracker.Contracts.QueryParams;

/// <summary>
/// Represents query parameters for the "GetWeightData" query.
/// </summary>
public sealed class GetWeightDataQueryParams
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetWeightDataQueryParams"/> class.
    /// </summary>
    /// <remarks>
    /// The constructor without parameters is required for model binding.
    /// </remarks>
    public GetWeightDataQueryParams()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetWeightDataQueryParams"/> class.
    /// </summary>
    /// <param name="dateFrom">The date from which to get the weight data.</param>
    /// <param name="dateTo">The date to which to get the weight data.</param>
    public GetWeightDataQueryParams(string? dateFrom, string? dateTo)
    {
        DateFrom = dateFrom;
        DateTo = dateTo;
    }

    /// <summary>
    /// Gets or initializes the date from which to get the weight data.
    /// </summary>
    /// <value>
    /// The date from which to get the weight data.
    /// </value>
    [FromQuery(Name = "date_from")]
    public string? DateFrom { get; init; }

    /// <summary>
    /// Gets or initializes the date to which to get the weight data.
    /// </summary>
    /// <value>
    /// The date to which to get the weight data.
    /// </value>
    [FromQuery(Name = "date_to")]
    public string? DateTo { get; init; }
}
