using chronos.notifications.core.Domain;
using chronos.notifications.core.Events;
using Microsoft.Extensions.Logging;

namespace chronos.notifications.core.Services.NotificationSenders;

internal sealed class EmployeeCreatedNotificationSender(
    ILogger<EmployeeCreatedNotificationSender> logger) : INotificationSender<EmployeeCreated>
{
    public Task SendAsync(
        EmployeeCreated message,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Employee with email: {Email} created", message.Email);
        return Task.CompletedTask;
    }
}