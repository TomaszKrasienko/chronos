namespace chronos.notifications.core.Services.NotificationSenders;

public interface INotificationSender<TMessage> where TMessage : class
{
    Task SendAsync(TMessage message, CancellationToken cancellationToken = default);
}