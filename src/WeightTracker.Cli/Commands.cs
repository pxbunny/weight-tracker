using Microsoft.AspNetCore.Mvc;
using WeightTracker.Cli.Authentication;
using WeightTracker.Client;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;

namespace WeightTracker.Cli;

internal static class Commands
{
    public static void RegisterCommands(this CoconaApp app)
    {
        app.AddCommand("login", LoginAsync);

        app.AddCommand("logout", Logout);

        app.AddCommand("get", GetWeightDataAsync);

        app.AddCommand("add", AddWeightDataAsync);

        app.AddCommand("update", UpdateWeightDataAsync);

        app.AddCommand("delete", DeleteWeightDataAsync);
    }

    private static async Task LoginAsync(
        [FromServices] AuthService authService)
    {
        await authService.AcquireTokenAsync();
    }

    private static void Logout(
        [FromServices] AuthService authService)
    {
        authService.ForgetToken();
    }

    private static async Task GetWeightDataAsync(
        [Option('f', Description = "Date from in format yyyy-MM-dd")] string? from,
        [Option('t', Description = "Date to in format yyyy-MM-dd")] string? to,
        [FromServices] IApiClient apiClient,
        [FromServices] AuthService authService)
    {
        var accessToken = authService.GetToken();

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            Console.WriteLine("Please login first.");
            return;
        }

        var queryParams = new GetWeightDataQueryParams(from, to);
        var data = await apiClient.GetWeightDataAsync(queryParams, accessToken);
        Console.WriteLine(data.AverageWeight);
    }

    private static async Task AddWeightDataAsync(
        [Option('d', Description = "Date in format yyyy-MM-dd")] string date,
        [Option('w', Description = "Weight value")] decimal weight,
        [FromServices] IApiClient apiClient,
        [FromServices] AuthService authService)
    {
        var accessToken = authService.GetToken();

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            Console.WriteLine("Please login first.");
            return;
        }

        var request = new AddWeightDataRequest(weight, date);
        await apiClient.AddWeightDataAsync(request, accessToken);
    }

    private static async Task UpdateWeightDataAsync(
        [Option('d', Description = "Date in format yyyy-MM-dd")] string date,
        [Option('w', Description = "Weight value")] decimal weight,
        [FromServices] IApiClient apiClient,
        [FromServices] AuthService authService)
    {
        var accessToken = authService.GetToken();

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            Console.WriteLine("Please login first.");
            return;
        }

        var request = new UpdateWeightDataRequest(weight);
        await apiClient.UpdateWeightDataAsync(date, request, accessToken);
    }

    private static async Task DeleteWeightDataAsync(
        [Option('d', Description = "Date in format yyyy-MM-dd")] string date,
        [FromServices] IApiClient apiClient,
        [FromServices] AuthService authService)
    {
        var accessToken = authService.GetToken();

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            Console.WriteLine("Please login first.");
            return;
        }

        await apiClient.DeleteWeightDataAsync(date, accessToken);
    }
}
