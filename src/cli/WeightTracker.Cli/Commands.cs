using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Spectre.Console;
using WeightTracker.ApiClient;
using WeightTracker.Cli.Authentication;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;

namespace WeightTracker.Cli;

/// <summary>
/// Contains the CLI commands.
/// </summary>
internal static class Commands
{
    /// <summary>
    /// An extension method to register CLI commands to the Cocona application.
    /// </summary>
    /// <param name="app">The Cocona application.</param>
    public static void RegisterCommands(this CoconaApp app)
    {
        app.AddCommand("login", LoginAsync)
            .WithDescription("Login to the application");

        app.AddCommand("logout", LogoutAsync)
            .WithDescription("Logout from the application");

        app.AddCommand("get", GetWeightDataAsync)
            .WithDescription("Get weight data for the specified date range");

        app.AddCommand("add", AddWeightDataAsync)
            .WithDescription("Add weight data for the specified date");

        app.AddCommand("update", UpdateWeightDataAsync)
            .WithDescription("Update weight data for the specified date");

        app.AddCommand("remove", RemoveWeightDataAsync)
            .WithDescription("Remove weight data for the specified date");
    }

    private static async Task LoginAsync(
        [FromServices] AuthService authService)
    {
        await StartAsync("Logging in...", async _ =>
        {
            await authService.AcquireTokenAsync();
            AnsiConsole.MarkupLine("Successfully logged in.");
        });
    }

    private static async Task LogoutAsync(
        [FromServices] AuthService authService)
    {
        await StartAsync("Logging out...", async _ =>
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
        await StartAsync("Fetching data...", authService, async (_, accessToken) =>
        {
            var queryParams = new GetWeightDataQueryParams(from, to);
            var response = await apiClient.GetWeightDataAsync(queryParams, accessToken);

            if (!response.Data.Any())
            {
                AnsiConsole.MarkupLine("No data found.");
                return;
            }

            var table = new Table();

            table.AddColumn("[bold]Date[/]");
            table.AddColumn("[bold]Weight[/]");
            table.AddColumn("[bold]+/-[/]");

            for (var i = 0; i < response.Data.Count(); i++)
            {
                var current = response.Data.ElementAt(i);
                var previous = i > 0
                    ? response.Data.ElementAt(i - 1)
                    : current;
                var diff = current.Weight - previous.Weight;

                var diffString = diff switch
                {
                    > 0 => $"[red]+{diff}[/]",
                    < 0 => $"[green]{diff}[/]",
                    _ => "0"
                };

                table.AddRow(
                    current.Date,
                    current.Weight.ToString(CultureInfo.InvariantCulture),
                    diffString);
            }

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine($"Average weight: [bold]{response.AverageWeight}[/]");
            AnsiConsole.MarkupLine($"Max weight:     [bold]{response.MaxWeight}[/]");
            AnsiConsole.MarkupLine($"Min weight:     [bold]{response.MinWeight}[/]");

            AnsiConsole.WriteLine();
        });
    }

    private static async Task AddWeightDataAsync(
        [Option('d', Description = "Date in format yyyy-MM-dd")] string? date,
        [Option('w', Description = "Weight value")] decimal weight,
        [FromServices] IApiClient apiClient,
        [FromServices] AuthService authService)
    {
        await StartAsync("Adding data...", authService, async (_, accessToken) =>
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
        var confirm = AnsiConsole.Confirm("Are you sure you want to update the data?");

        if (!confirm)
        {
            return;
        }

        await StartAsync("Updating data...", authService, async (_, accessToken) =>
        {
            var request = new UpdateWeightDataRequest(weight);
            await apiClient.UpdateWeightDataAsync(date, request, accessToken);
            AnsiConsole.MarkupLine("Successfully updated data.");
        });
    }

    private static async Task RemoveWeightDataAsync(
        [Option('d', Description = "Date in format yyyy-MM-dd")] string date,
        [FromServices] IApiClient apiClient,
        [FromServices] AuthService authService)
    {
        var confirm = AnsiConsole.Confirm("Are you sure you want to remove the data?");

        if (!confirm)
        {
            return;
        }

        await StartAsync("Deleting data...", authService, async (_, accessToken) =>
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
