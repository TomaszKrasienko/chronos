using System.Text;
using System.Text.Json;
using chronos.shared.messaging.rabbit_mq.AlternateExchange;
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
        string? messageId = null,
        AsyncEventHandler<BasicReturnEventArgs>? basicReturn = null,
        CancellationToken? cancellationToken = null) where T : class, IMessage
    {
        var channel = channelFactory.ProducerChannel;
        var jsonContent = JsonSerializer.Serialize(message, message.GetType());
        var payload = Encoding.UTF8.GetBytes(jsonContent);

        var basicProperties = new BasicProperties
        {
            Type = message.GetType().FullName,
            MessageId = messageId
        };
        
        if (basicReturn != null)
        {
            channel.BasicReturnAsync += basicReturn;
        }
        
        var arguments = new Dictionary<string, object?>();

#pragma warning disable CS0162 // Unreachable code detected
        //At this moment we don't need to declare alternate exchange
        if (false)
        {
            AlternateExchangeSetup.AddAlternateExchangeArgument(arguments);

            await AlternateExchangeSetup.DeclareAlternateExchangeAsync(
                channel,
                cancellationToken ?? CancellationToken.None);
        }
#pragma warning restore CS0162 // Unreachable code detected

        await channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: arguments,
            cancellationToken: cancellationToken ?? CancellationToken.None);
        try
        {
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory:  true,
                basicProperties: basicProperties,
                body: payload,
                cancellationToken: cancellationToken ?? CancellationToken.None);   
        }
        catch (Exception ex)
        {
            throw new Exception($"Error publishing message: {ex.Message}", ex);
        }
    }
}