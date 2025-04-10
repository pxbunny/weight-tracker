namespace WeightTracker.Core;

public static class DateOnlyExtensions
{
    private const string DateFormat = "yyyy-MM-dd";

    public static string ToFormattedString(this DateOnly date) => date.ToString(DateFormat); // TODO: change name
}
