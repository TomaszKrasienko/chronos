using chronos.notifications.core.DAL;
using chronos.notifications.core.Domain;
using chronos.notifications.core.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace chronos.notifications.core.Services.NotificationSenders;

internal sealed class TimeLogCreatedNotificationSender(
    ILogger<TimeLogCreatedNotificationSender> logger,
    NotificationsDbContext dbContext,
    TimeProvider timeProvider)
    : INotificationSender<TimeLogCreated>
{
    public async Task SendAsync(
        TimeLogCreated message,
        CancellationToken cancellationToken = default)
    {
        var contact = await dbContext
            .Contacts
            .SingleOrDefaultAsync(x => x.Id == message.EmployeeId, cancellationToken);

        if (contact is null)
        {
            return;
        }

        var notificationMessage = NotificationMessage.Create(
            Ulid.NewUlid(),
            contact.Supervisor,
            "New time log to acceptance",
            "Check your subordinate time logs",
            timeProvider);
        
        await dbContext.NotificationMessages.AddAsync(notificationMessage, cancellationToken);
        
        logger.LogInformation(
            "Sending notification for TimeLog created: {TimeLogId} for Employee: {EmployeeId}, Topic: {Topic}, Duration: {Duration}, Notes: {Notes}, Supervisor: {Supervisor}",
            message.Id,
            message.EmployeeId,
            message.Topic,
            message.TimeSpan,
            message.Notes,
            contact.Supervisor);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}