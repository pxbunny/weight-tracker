namespace WeightTracker.WebApi.Extensions;

/// <summary>
/// Contains the extension methods for <see cref="DateOnly"/>.
/// </summary>
internal static class DateOnlyExtensions
{
    /// <summary>
    /// Converts the date to a formatted string.
    /// </summary>
    /// <remarks>
    /// To standardize the date format used within the application,
    /// this method converts the date to a string in the format "yyyy-MM-dd".
    /// </remarks>
    /// <param name="date">The date only value to convert.</param>
    /// <returns>The formatted string.</returns>
    public static string ToFormattedString(this DateOnly date) => date.ToString("yyyy-MM-dd");
}
