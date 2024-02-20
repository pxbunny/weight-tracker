using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Spectre.Console;
using WeightTracker.Cli.Authentication;
using WeightTracker.Client;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;

namespace WeightTracker.Cli;

internal static class Commands
{
    public static void RegisterCommands(this CoconaApp app)
    {
        app.AddCommand("login", LoginAsync)
            .WithDescription("Login to the application");

        app.AddCommand("logout", LogoutAsync)
            .WithDescription("Logout from the application");

        app.AddCommand("get", GetWeightDataAsync)
            .WithDescription("Get weight data");

        app.AddCommand("add", AddWeightDataAsync)
            .WithDescription("Add weight data");

        app.AddCommand("update", UpdateWeightDataAsync)
            .WithDescription("Update weight data");

        app.AddCommand("delete", DeleteWeightDataAsync)
            .WithDescription("Delete weight data");
    }

    private static async Task LoginAsync(
        [FromServices] AuthService authService)
    {
        await StartAsync("Logging in...", async ctx =>
        {
            await authService.AcquireTokenAsync();
            AnsiConsole.MarkupLine("Successfully logged in.");
        });
    }

    private static async Task LogoutAsync(
        [FromServices] AuthService authService)
    {
        await StartAsync("Logging out...", async ctx =>
        {
            await authService.ForgetTokenAsync();
            AnsiConsole.MarkupLine("Successfully logged out.");
        });
    }

    private static async Task GetWeightDataAsync(
        [Option('f', Description = "Date from in format yyyy-MM-dd")] string? from,
        [Option('t', Description = "Date to in format yyyy-MM-dd")] string? to,
        [FromServices] IApiClient apiClient,
        [FromServices] AuthService authService)
    {
        await StartAsync("Fetching data...", authService, async (ctx, accessToken) =>
        {
            var queryParams = new GetWeightDataQueryParams(from, to);
            var response = await apiClient.GetWeightDataAsync(queryParams, accessToken);

            if (!response.Data.Any())
            {
                AnsiConsole.MarkupLine("No data found.");
                return;
            }

            var table = new Table();

            table.AddColumn("Date");
            table.AddColumn("Weight");
            table.AddColumn("+/-");

            for (var i = 0; i < response.Data.Count(); i++)
            {
                var current = response.Data.ElementAt(i);
                var previous = i > 0
                    ? response.Data.ElementAt(i - 1)
                    : current;
                var diff = current.Weight - previous.Weight;

                var diffString = diff switch
                {
                    > 0 => $"[green]+{diff}[/]",
                    < 0 => $"[red]{diff}[/]",
                    _ => "0"
                };

                table.AddRow(
                    current.Date,
                    current.Weight.ToString(CultureInfo.InvariantCulture),
                    diffString);
            }

            AnsiConsole.Write(table);

            AnsiConsole.MarkupLine($"Average weight: [bold]{response.AverageWeight}[/]");
            AnsiConsole.MarkupLine($"Max weight: [bold]{response.MaxWeight}[/]");
            AnsiConsole.MarkupLine($"Min weight: [bold]{response.MinWeight}[/]");
        });
    }

    private static async Task AddWeightDataAsync(
        [Option('d', Description = "Date in format yyyy-MM-dd")] string date,
        [Option('w', Description = "Weight value")] decimal weight,
        [FromServices] IApiClient apiClient,
        [FromServices] AuthService authService)
    {
        await StartAsync("Adding data...", authService, async (ctx, accessToken) =>
        {
            var request = new AddWeightDataRequest(weight, date);
            await apiClient.AddWeightDataAsync(request, accessToken);
            AnsiConsole.MarkupLine("Successfully added data.");
        });
    }

    private static async Task UpdateWeightDataAsync(
        [Option('d', Description = "Date in format yyyy-MM-dd")] string date,
        [Option('w', Description = "Weight value")] decimal weight,
        [FromServices] IApiClient apiClient,
        [FromServices] AuthService authService)
    {
        await StartAsync("Updating data...", authService, async (ctx, accessToken) =>
        {
            var request = new UpdateWeightDataRequest(weight);
            await apiClient.UpdateWeightDataAsync(date, request, accessToken);
            AnsiConsole.MarkupLine("Successfully updated data.");
        });
    }

    private static async Task DeleteWeightDataAsync(
        [Option('d', Description = "Date in format yyyy-MM-dd")] string date,
        [FromServices] IApiClient apiClient,
        [FromServices] AuthService authService)
    {
        await StartAsync("Deleting data...", authService, async (ctx, accessToken) =>
        {
            await apiClient.DeleteWeightDataAsync(date, accessToken);
            AnsiConsole.MarkupLine("Successfully deleted data.");
        });
    }

    private static async Task StartAsync(
        string message,
        Func<StatusContext, Task> action)
    {
        await AnsiConsole.Status()
            .SpinnerStyle(Style.Plain)
            .StartAsync(message, async ctx =>
            {
                await action(ctx);
            });
    }

    private static async Task StartAsync(
        string message,
        AuthService authService,
        Func<StatusContext, string, Task> action)
    {
        await AnsiConsole.Status()
            .SpinnerStyle(Style.Plain)
            .StartAsync(message, async ctx =>
            {
                var accessToken = authService.GetToken();

                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    AnsiConsole.MarkupLine("Please login first.");
                    return;
                }

                await action(ctx, accessToken);
            });
    }
}
