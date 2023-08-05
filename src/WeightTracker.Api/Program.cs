using System.Reflection;
using Mapster;
using Microsoft.Extensions.Azure;
using WeightTracker.Api.Conventions;
using WeightTracker.Api.Interfaces;
using WeightTracker.Api.Services;

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(clientBuilder =>
{
    var storageConnectionString = builder.Configuration.GetSection("AzureWebJobsStorage").Value;
    clientBuilder.AddTableServiceClient(storageConnectionString);
});

builder.Services.AddScoped<IWeightDataService, WeightDataService>();

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new KebabCaseRouteConvention());
    options.Conventions.Add(new RoutePrefixConvention("api"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();

app.Run();
