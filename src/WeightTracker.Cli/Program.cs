using WeightTracker.Cli.Authentication;
using WeightTracker.Client;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddApiClient(string.Empty);

var app = builder.Build();

app.AddCommand("login", async (AuthService authService) =>
{
    await authService.AcquireTokenAsync();
});

app.AddCommand("logout", (AuthService authService) =>
{
    authService.ForgetToken();
});

app.AddCommand("test", () =>
{
    var authToken = Environment.GetEnvironmentVariable("AUTH_TOKEN", EnvironmentVariableTarget.User);
    Console.WriteLine($"Token: {authToken}");
});

app.Run();
