namespace chronos.shared.messaging.rabbit_mq;

internal sealed class RabbitMqMessageDispatcher(
    IMessagePublisher messagePublisher,
    ISendingNameConvention sendingNameConvention) : IMessageDispatcher
{
    public async Task Send<T>(T message, CancellationToken? cancellationToken = null) where T : class, IMessage
    {
        var messageRoute = sendingNameConvention.GetExchangeAndRoutingKey(message);

        await messagePublisher
            .Send(message,
                messageRoute.exchange,
                string.Empty,
                null,
                null,
                cancellationToken);
    }
}