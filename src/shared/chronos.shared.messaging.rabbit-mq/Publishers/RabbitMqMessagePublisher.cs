using System.Text;
using System.Text.Json;
using chronos.shared.messaging.rabbit_mq.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace chronos.shared.messaging.rabbit_mq.Publishers;

internal sealed class RabbitMqMessagePublisher(
    RabbitMqChannelFactory channelFactory) : IMessagePublisher
{
    public async Task Send<T>(
        T message,
        string exchange,
        string routingKey,
        AsyncEventHandler<BasicReturnEventArgs>? basicReturn,
        CancellationToken? cancellationToken = null) where T : class
    {
        
        var channel = channelFactory.ProducerChannel;
        var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var basicProperties = new BasicProperties
        {
            Type = typeof(T).FullName,
        };


        if (basicReturn != null)
        {
            channel.BasicReturnAsync += basicReturn;
        }

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