using chronos.notifications.core.DAL;
using chronos.notifications.core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace chronos.notifications.core.Services;

public interface INotificationMessagesService
{
    Task<IReadOnlyCollection<NotificationMessageResponseDto>> GetUnreadAsync(
        Ulid employeeId,
        CancellationToken cancellationToken = default);
}

internal sealed class NotificationMessagesService(
    NotificationsDbContext dbContext) : INotificationMessagesService
{
    public async Task<IReadOnlyCollection<NotificationMessageResponseDto>> GetUnreadAsync(
        Ulid employeeId,
        CancellationToken cancellationToken = default)
    {
        var notifications = await dbContext
            .NotificationMessages
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId && x.ReadAt == null)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x =>
                new NotificationMessageResponseDto(
                    x.Id,
                    x.EmployeeId,
                    x.Topic,
                    x.Message,
                    x.CreatedAt,
                    x.ReadAt))
            .ToListAsync(cancellationToken);

        return notifications;
    }
}
