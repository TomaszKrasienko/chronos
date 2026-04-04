using RabbitMQ.Client.Events;

namespace chronos.shared.messaging;

public interface IMessagePublisher
{
    Task Send<T>(
        T message,
        string exchange,
        string routingKey,
        AsyncEventHandler<BasicReturnEventArgs>? basicReturn,
        CancellationToken? cancellationToken = null) where T : class;
}