using Azure.Data.Tables;
using WeightTracker.Api.Interfaces;
using WeightTracker.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var storageConnectionString = builder.Configuration.GetSection("AzureWebJobsStorage").Value;

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
        
builder.Services.AddScoped(_ => new TableServiceClient(storageConnectionString));
builder.Services.AddScoped<IWeightDataService, WeightDataService>();

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
// app.UseAuthorization();
app.MapControllers();

app.Run();
