using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace WeightTracker.Client.Client;

internal static class ObjectExtensions
{
    private const string AttributeName = nameof(FromQueryAttribute);
    private const string AttributePropertyName = nameof(FromQueryAttribute.Name);

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
