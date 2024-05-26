namespace WeightTracker.Func.Notifications;

public sealed class NotificationOptions
{
    public const string Position = "Notifications";

    public string EmailHost { get; set; } = string.Empty;

    public int EmailPort { get; set; }

    public string EmailPassword { get; set; } = string.Empty;

    public string SenderEmail { get; set; } = string.Empty;

    public string SenderName { get; set; } = string.Empty;
}
