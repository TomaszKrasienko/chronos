using chronos.shared.messaging.rabbit_mq.Configuration.Options;
using chronos.shared.messaging.rabbit_mq.Connections;
using chronos.shared.messaging.rabbit_mq.Consumers;
using chronos.shared.messaging.rabbit_mq.Publishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace chronos.shared.messaging.rabbit_mq.Configuration;

public static class RabbitMqServicesExtensions
{
    public static IServiceCollection AddChronosRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration,
        string appName)
        => services
            .AddOptions(configuration)
            .AddConnections(appName)
            .AddTransient<RabbitMqChannelFactory>()
            .AddSingleton<ISendingNameConvention, SendingNameConvention>()
            .AddSingleton<IMessagesRouteRegistry, MessagesRouteRegistry>()
            .AddSingleton<IConsumingNameConvention, ConsumingNameConvention>()
            .AddTransient<IMessagePublisher, RabbitMqMessagePublisher>();
    
    public static IServiceCollection AddConsumer<TMessage>(
        this IServiceCollection services,
        Func<IServiceProvider, Func<TMessage, CancellationToken, string?, Task>> handle) where TMessage : class
    {
        services.AddHostedService(sp =>
        {
            var handler = handle(sp);
            return new RabbitMqConsumer<TMessage>(sp, handler);
        });
        return services;
    }

    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)));
    
    private static IServiceCollection AddConnections(
        this IServiceCollection services,
        string appName)
    {
        services.AddSingleton(sp =>
        {
            var rabbitMqOptions = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
            
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqOptions.HostName,
                UserName = rabbitMqOptions.Username,
                Password = rabbitMqOptions.Password,
                VirtualHost = rabbitMqOptions.VirtualHost,
                Port = rabbitMqOptions.Port
            };

            var consumerConnection = factory.CreateConnectionAsync($"{appName}.Consumer").GetAwaiter().GetResult();
            var producerConnection = factory.CreateConnectionAsync($"{appName}.Producer").GetAwaiter().GetResult();
            
            return new RabbitMqConnectionProvider(consumerConnection, producerConnection);
        });
        
        return services;
    }
}