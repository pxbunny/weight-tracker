using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeightTracker.Functions.Notifications;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<INotificationService, NotificationService>();

        services.Configure<NotificationOptions>(context.Configuration.GetSection(NotificationOptions.Position));
    })
    .Build();

host.Run();
