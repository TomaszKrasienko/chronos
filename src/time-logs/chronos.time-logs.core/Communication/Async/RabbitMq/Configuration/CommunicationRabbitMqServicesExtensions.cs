using chronos.shared.configuration.Options;
using chronos.shared.messaging.rabbit_mq.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class CommunicationRabbitMqServicesExtensions
{
    internal static IServiceCollection AddRabbitMq(this IServiceCollection services,
        IConfiguration configuration)
    {
        var appOptions = services
            .GetOptions<AppOptions>();
        
        services.AddChronosRabbitMq(
            configuration,
            appOptions.Name);
        
        return services;
    }
}