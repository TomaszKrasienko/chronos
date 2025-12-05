using chronos.shared.messaging.rabbitmq.Configuration.Options;
using chronos.shared.messaging.rabbitmq.Connections;
using chronos.shared.messaging.rabbitmq.Publishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace chronos.shared.messaging.rabbitmq.Configuration;

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
            .AddTransient<IMessagePublisher, RabbitMqMessagePublisher>()
    ;

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