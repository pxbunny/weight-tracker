using System.Text.RegularExpressions;

namespace WeightTracker.Cli.Extensions;

public static partial class StringExtensions
{
    public static string ToKebabCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return GetRegex().Replace(value, "-$1")
            .Trim()
            .ToLower();
    }

    [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])", RegexOptions.Compiled)]
    private static partial Regex GetRegex();
}
