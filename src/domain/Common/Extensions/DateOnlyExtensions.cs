namespace WeightTracker.Domain.Common.Extensions;

/// <summary>
/// Contains the extension methods for <see cref="DateOnly"/>.
/// </summary>
public static class DateOnlyExtensions
{
    /// <summary>
    /// The date format.
    /// </summary>
    /// <remarks>
    /// The format used to convert the date to a string.
    /// It's an ISO 8601 format with the year, month, and day.
    /// </remarks>
    private const string DateFormat = "yyyy-MM-dd";

    /// <summary>
    /// Converts the date to a formatted string.
    /// </summary>
    /// <remarks>
    /// To standardize the date format used within the application,
    /// this method converts the date to a string in the format "yyyy-MM-dd".
    /// </remarks>
    /// <param name="date">The date only value to convert.</param>
    /// <returns>The formatted string.</returns>
    public static string ToFormattedString(this DateOnly date) => date.ToString(DateFormat);
}
