using System.Reflection;
using Mapster;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web;
using WeightTracker.WebApi;
using WeightTracker.WebApi.Services;

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddTableServiceClient(builder.Configuration["AzureWebJobsStorage"]);
});

builder.Services.AddScoped<IWeightDataService, WeightDataService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(Swagger.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.RegisterEndpoints();

app.Run();
