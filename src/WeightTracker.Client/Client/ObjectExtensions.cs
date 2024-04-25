using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace WeightTracker.Client.Client;

/// <summary>
/// Contains the extension methods for objects and generics.
/// </summary>
internal static class ObjectExtensions
{
    private const string AttributeName = nameof(FromQueryAttribute);
    private const string AttributePropertyName = nameof(FromQueryAttribute.Name);

    /// <summary>
    /// Builds a query string from the object properties that have the <see cref="FromQueryAttribute"/>.
    /// </summary>
    /// <param name="obj">The object to build the query string from.</param>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>The query string.</returns>
    /// <example>
    /// <code>
    /// var queryParams = new GetWeightDataQueryParams { Date = "2021-01-01" };
    /// var queryString = queryParams.BuildQueryString();
    /// </code>
    /// </example>
    public static string BuildQueryString<T>(this T obj)
        where T : class
    {
        var parametersData = obj.GetType().GetProperties()
            .Where(p => p.CustomAttributes.Any(a => a.AttributeType.Name == AttributeName))
            .Select(p => (GetPropertyName(p), p.GetValue(obj)?.ToString()));

        var query = HttpUtility.ParseQueryString(string.Empty);

        foreach (var (key, value) in parametersData)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            query[key] = value;
        }

        return query.ToString() ?? string.Empty;
    }

    private static string GetPropertyName(MemberInfo property) =>
        property.CustomAttributes.First(a => a.AttributeType.Name == AttributeName).NamedArguments
            .First(na => na.MemberName == AttributePropertyName).TypedValue.Value as string ?? property.Name;
}
