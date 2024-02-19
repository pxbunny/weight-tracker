using WeightTracker.Cli;
using WeightTracker.Cli.Authentication;
using WeightTracker.Client;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddApiClient("https://localhost:7081");

var app = builder.Build();

app.RegisterCommands();

app.Run();
