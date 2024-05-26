using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace WeightTracker.Func.Notifications;

public interface INotificationService
{
    Task SendReminderAsync(string userId);
}

internal sealed class NotificationService(IOptions<NotificationOptions> options) : INotificationService
{
    public async Task SendReminderAsync(string userId)
    {
        var senderEmail = options.Value.SenderEmail;
        var senderName = options.Value.SenderName;
        var password = options.Value.EmailPassword;
        var recipientEmail = options.Value.SenderEmail;
        var recipientName = options.Value.SenderName;
        var subject = "Weight Tracker Reminder";
        var body = "Just testing the reminder email.";

        await SendEmailAsync(senderEmail, senderName, password, recipientEmail, recipientName, subject, body);
    }

    private async Task SendEmailAsync(
        string senderEmail,
        string senderName,
        string password,
        string recipientEmail,
        string recipientName,
        string subject,
        string body)
    {
        var host = options.Value.EmailHost;
        var port = options.Value.EmailPort;

        using var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, senderEmail));
        message.To.Add(new MailboxAddress(recipientName, recipientEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(senderEmail, password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
