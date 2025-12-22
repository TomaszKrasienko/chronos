using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace chronos.notifications.core.Services.NotificationSenders;

public interface INotificationSenderFactory
{
    INotificationSender<TMessage>? GetInstance<TMessage>() where TMessage : class;
}

internal sealed class NotificationSenderFactory(
    ILogger<NotificationSenderFactory> logger,
    IServiceProvider serviceProvider) : INotificationSenderFactory
{
    public INotificationSender<TMessage>? GetInstance<TMessage>() where TMessage : class
    {
        var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetService<INotificationSender<TMessage>>();

        if (service is null)
        {
            logger.LogWarning("No notification sender has been registered.");
            return null;
        }
        
        return service;
    }
}