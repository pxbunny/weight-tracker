namespace WeightTracker.Api.Models;

/// <summary>
/// Represents the data filter.
/// </summary>
/// <remarks>
/// This class is used to filter weight data based on the user ID and date range.
/// </remarks>
internal sealed class DataFilter
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    /// <value>The user ID.</value>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start date.
    /// </summary>
    /// <value>The start date.</value>
    public DateOnly? DateFrom { get; set; }

    /// <summary>
    /// Gets or sets the end date.
    /// </summary>
    /// <value>The end date.</value>
    public DateOnly? DateTo { get; set; }

    /// <summary>
    /// Deconstructs the data filter.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="dateFrom">The start date.</param>
    /// <param name="dateTo">The end date.</param>
    /// <example>
    /// <code>
    /// var (userId, dateFrom, dateTo) = dataFilter;
    /// </code>
    /// </example>
    public void Deconstruct(
        out string userId,
        out DateOnly? dateFrom,
        out DateOnly? dateTo)
    {
        userId = UserId;
        dateFrom = DateFrom;
        dateTo = DateTo;
    }
}
