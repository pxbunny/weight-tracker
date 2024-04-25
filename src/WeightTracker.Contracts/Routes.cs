namespace WeightTracker.Contracts;

/// <summary>
/// Contains the routes for the API.
/// </summary>
/// <remarks>
/// The parameters in the routes are enclosed in curly braces.
/// For now date is the only parameter in the routes.
/// </remarks>
/// <example>
/// You can replace the date parameter in the route like this:
/// <code>
/// var uri = Routes.UpdateWeightData.Replace("{date}", date);
/// </code>
/// </example>
public static class Routes
{
    public const string AddWeightData = "/api/weight";
    public const string GetWeightData = "/api/weight";
    public const string UpdateWeightData = "/api/weight/{date}";
    public const string DeleteWeightData = "/api/weight/{date}";
}
