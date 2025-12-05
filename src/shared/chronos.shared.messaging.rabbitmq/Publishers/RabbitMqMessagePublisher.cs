using System.Text;
using System.Text.Json;
using chronos.shared.messaging.rabbitmq.Connections;
using RabbitMQ.Client;

namespace chronos.shared.messaging.rabbitmq.Publishers;

internal sealed class RabbitMqMessagePublisher(
    RabbitMqChannelFactory channelFactory,
    ISendingNameConvention sendingNameConvention) : IMessagePublisher
{
    public async Task Send<T>(
        T message,
        CancellationToken? cancellationToken = null) where T : class
    {
        var channel = channelFactory.ProducerChannel;
        var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var basicProperties = new BasicProperties
        {
            Type = typeof(T).FullName,
        };

        var (exchange, routingKey) = sendingNameConvention
            .GetExchangeAndRoutingKey(message);

        await channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken ?? CancellationToken.None);

        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            mandatory:  true,
            basicProperties: basicProperties,
            body: payload,
            cancellationToken: cancellationToken ?? CancellationToken.None);
    }
}