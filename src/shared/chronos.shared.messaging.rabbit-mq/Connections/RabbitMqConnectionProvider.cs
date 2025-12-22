using RabbitMQ.Client;

namespace chronos.shared.messaging.rabbit_mq.Connections;

internal sealed class RabbitMqConnectionProvider(IConnection consumerConnection, IConnection producerConnection)
{
    public IConnection ConsumerConnection { get; } = consumerConnection;
    public IConnection ProducerConnection { get; } = producerConnection;
}