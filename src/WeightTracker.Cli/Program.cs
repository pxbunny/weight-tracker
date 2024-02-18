using WeightTracker.Cli.Authentication;
using WeightTracker.Client;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddApiClient("https://localhost:7081");

var app = builder.Build();

app.AddCommand("login", async (
    AuthService authService) =>
{
    await authService.AcquireTokenAsync();
});

app.AddCommand("logout", (
    AuthService authService) =>
{
    authService.ForgetToken();
});

app.AddCommand("get", async (
    IApiClient apiClient,
    AuthService authService) =>
{
    var accessToken = authService.GetToken();

    if (accessToken is null)
    {
        Console.WriteLine("Please login first.");
        return;
    }

    var queryParams = new GetWeightDataQueryParams
    {
        DateFrom = null,
        DateTo = null
    };

    var data = await apiClient.GetWeightDataAsync(queryParams, accessToken);
    Console.WriteLine(data.AverageWeight);
});

app.AddCommand("add", async (
    decimal weight,
    string date,
    IApiClient apiClient,
    AuthService authService) =>
{
    var accessToken = authService.GetToken();

    if (accessToken is null)
    {
        Console.WriteLine("Please login first.");
        return;
    }

    var request = new AddWeightDataRequest
    {
        Weight = weight,
        Date = date
    };
    await apiClient.AddWeightDataAsync(request, accessToken);
});

app.AddCommand("update", async (
    string date,
    decimal weight,
    IApiClient apiClient,
    AuthService authService) =>
{
    var accessToken = authService.GetToken();

    if (accessToken is null)
    {
        Console.WriteLine("Please login first.");
        return;
    }

    var request = new UpdateWeightDataRequest
    {
        Weight = weight
    };
    await apiClient.UpdateWeightDataAsync(date, request, accessToken);
});

app.AddCommand("delete", async (
    string date,
    IApiClient apiClient,
    AuthService authService) =>
{
    var accessToken = authService.GetToken();

    if (accessToken is null)
    {
        Console.WriteLine("Please login first.");
        return;
    }

    await apiClient.DeleteWeightDataAsync(date, accessToken);
});

app.Run();
