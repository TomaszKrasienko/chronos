using RabbitMQ.Client;

namespace chronos.shared.messaging.rabbitmq.Connections;

internal sealed class RabbitMqConnectionProvider(IConnection consumerConnection, IConnection producerConnection)
{
    public IConnection ConsumerConnection { get; } = consumerConnection;
    public IConnection ProducerConnection { get; } = producerConnection;
}