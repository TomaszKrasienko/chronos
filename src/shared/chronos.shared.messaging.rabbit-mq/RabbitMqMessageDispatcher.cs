namespace chronos.shared.messaging.rabbit_mq;

internal sealed class RabbitMqMessageDispatcher(
    IMessagePublisher messagePublisher,
    IMessagesRouteRegistry messagesRouteRegistry) : IMessageDispatcher
{
    public async Task Send<T>(T message, CancellationToken? cancellationToken = null) where T : class, IMessage
    {
        var messageRoute = messagesRouteRegistry
            .GetRoute<T>();

        List<Task> sendingTasks = [];

        if (messageRoute.RoutingKeys.Count == 0)
        {
            sendingTasks.Add(messagePublisher
                .Send(message,
                    messageRoute.Exchange,
                    string.Empty,
                    null,
                    cancellationToken));
        }
        
        foreach (var routingKey in messageRoute.RoutingKeys)
        {
            sendingTasks.Add(messagePublisher
                .Send(message,
                    messageRoute.Exchange,
                    routingKey,
                    null,
                    cancellationToken));
        }
        
        await Task.WhenAll(sendingTasks);
    }
}