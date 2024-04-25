using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WeightTracker.Cli;
using WeightTracker.Cli.Authentication;
using WeightTracker.Client;

var builder = CoconaApp.CreateBuilder();

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("wtrack.config.json", optional: false)
    .AddJsonFile("wtrack.config.dev.json", optional: true);

// Clear the default logging providers to avoid logging to the console.
// This is done to keep the output clean.
builder.Logging.ClearProviders();

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddApiClient(builder.Configuration["ApiClient:BaseUrl"]);

var app = builder.Build();

app.RegisterCommands();

app.Run();
