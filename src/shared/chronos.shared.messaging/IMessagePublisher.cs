using RabbitMQ.Client.Events;

namespace chronos.shared.messaging;

public interface IMessagePublisher
{
    Task Send<T>(
        T message,
        string exchange,
        string routingKey,
        string? messageId = null,
        AsyncEventHandler<BasicReturnEventArgs>? basicReturn = null,
        CancellationToken? cancellationToken = null) where T : class, IMessage;
}