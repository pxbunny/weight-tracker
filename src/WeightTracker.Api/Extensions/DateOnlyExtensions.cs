namespace WeightTracker.Api.Extensions;

internal static class DateOnlyExtensions
{
    public static string ToFormattedString(this DateOnly date) => date.ToString("yyyy-MM-dd");
}
