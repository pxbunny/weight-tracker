using System.Reflection;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web;
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

#pragma warning disable SA1512 // TODO: remove pragma after adding authentication

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

#pragma warning restore SA1512

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#pragma warning disable SA1512 // TODO: remove pragma after adding authentication

// app.UseAuthorization();

#pragma warning restore SA1512

app.MapControllers();

app.Run();
