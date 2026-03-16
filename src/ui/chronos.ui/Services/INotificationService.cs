using chronos.ui.Models;

namespace chronos.ui.Services;

public interface INotificationService
{
    Task<IReadOnlyCollection<NotificationMessage>> GetUnreadAsync(string employeeId);
    Task MarkAsReadAsync(string notificationId, string employeeId);
}
