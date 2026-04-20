using System.Text.Json;
using chronos.shared.messaging.rabbit_mq.AlternateExchange;
using chronos.shared.messaging.rabbit_mq.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace chronos.shared.messaging.rabbit_mq.Consumers;

internal sealed class RabbitMqConsumer<TMessage> : IHostedService where TMessage : class
{
    private readonly ILogger<RabbitMqConsumer<TMessage>> _logger;
    private readonly IServiceScope _scope;
    private readonly RabbitMqChannelFactory _rabbitMqChannelFactory;
    private readonly IMessagesRouteRegistry _messagesRouteRegistry;
    private readonly IConsumingNameConvention _convention;
    private readonly Func<TMessage, CancellationToken, string?, Task> _handle;
    private readonly bool _messageAutoConfirm;

    public RabbitMqConsumer(
        IServiceProvider serviceProvider,
        Func<TMessage, CancellationToken, string?, Task> handle,
        bool autoConfirm = false)
    {
        _scope = serviceProvider.CreateScope();
        _logger = _scope.ServiceProvider.GetRequiredService<ILogger<RabbitMqConsumer<TMessage>>>();
        _rabbitMqChannelFactory = _scope.ServiceProvider.GetRequiredService<RabbitMqChannelFactory>();
        _messagesRouteRegistry = _scope.ServiceProvider.GetRequiredService<IMessagesRouteRegistry>();
        _convention = _scope.ServiceProvider.GetRequiredService<IConsumingNameConvention>();
        _handle = handle;
        _messageAutoConfirm = autoConfirm;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var channel = _rabbitMqChannelFactory.ConsumerChannel;
        var consumer = new AsyncEventingBasicConsumer(channel);

        var (exchange, _, routingKeys, isTemporary) = _messagesRouteRegistry.GetRoute<TMessage>();
        
        var queue = isTemporary 
            ? _convention.GetTemporaryQueueName<TMessage>()
            : _convention.GetQueueName<TMessage>();
        
        await InitializeTopology(
            channel,
            exchange,
            queue,
            isTemporary,
            routingKeys,
            cancellationToken);
        
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                _logger.LogInformation("Received message of type: {messageType}", typeof(TMessage).Name);
                var message = JsonSerializer.Deserialize<TMessage>(ea.Body.ToArray());

                if (message is null)
                {
                    return;
                }

                await _handle(message, cancellationToken, ea.BasicProperties.Type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await channel.BasicNackAsync(ea.DeliveryTag, false, true, ea.CancellationToken);
                return;
            }
            
            await channel.BasicAckAsync(
                deliveryTag: ea.DeliveryTag,
                multiple: false,
                cancellationToken: ea.CancellationToken);
        };
        
        _ = await channel.BasicConsumeAsync(
            queue: queue,
            autoAck: _messageAutoConfirm,
            consumerTag: "",
            noLocal: false,
            exclusive: false,
            consumer: consumer,
            cancellationToken: cancellationToken,
            arguments: null);
        
        _logger.LogInformation($"Consumer for {typeof(TMessage)} started");
    }
    
    private async Task InitializeTopology(
        IChannel channel,
        string exchange,
        string queue,
        bool isTemporary,
        List<string> routingKeys,
        CancellationToken cancellationToken = default)
    {
        var arguments = new Dictionary<string, object?>();
        
#pragma warning disable CS0162 // Unreachable code detected
        //At this moment we don't need to declare alternate exchange
        if (false)
        {
            AlternateExchangeSetup.AddAlternateExchangeArgument(arguments);

            await AlternateExchangeSetup.DeclareAlternateExchangeAsync(
                channel,
                cancellationToken);
        }
#pragma warning restore CS0162 // Unreachable code detected

        await channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: arguments,
            cancellationToken: cancellationToken);
        
        await channel.QueueDeclareAsync(
            queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object?>
            {
                ["x-expires"] = (int)TimeSpan.FromSeconds(10).TotalMilliseconds
            },
            cancellationToken: cancellationToken);

        foreach (var routingKey in routingKeys)
        {
            await channel.QueueBindAsync(
                queue: queue,
                exchange: exchange,
                routingKey: routingKey,
                cancellationToken: cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _scope.Dispose();
        return Task.CompletedTask;
    }
}