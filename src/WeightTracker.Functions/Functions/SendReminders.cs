using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using WeightTracker.Functions.Notifications;

namespace WeightTracker.Functions.Functions;

public class SendReminders(
    INotificationService notificationService,
    ILogger<SendReminders> logger)
{
    private const string FunctionName = "send-reminders-func";

    [Function(FunctionName)]
    public async Task RunAsync([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timer)
    {
        logger.LogInformation("'{functionName}' started at: {time}", FunctionName, DateTimeOffset.Now);

        await notificationService.SendReminderAsync(string.Empty);

        logger.LogInformation("'{functionName}' completed at: {time}", FunctionName, DateTimeOffset.Now);

        if (timer.ScheduleStatus is not null)
        {
            logger.LogInformation("Next '{functionName}' call scheduled at: {time}", FunctionName, timer.ScheduleStatus.Next);
        }
    }
}
