using chronos.shared.configuration.Options;
using chronos.shared.messaging.rabbit_mq.Configuration;
using chronos.time_logs.core.Events.External;
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

        services.AddConsumer<TimeLogAutomaticallyAccepted>(sp =>
        {
            return async (msg, ct, _) =>
            {
                using var scope = sp.CreateScope();
                var handler = scope.ServiceProvider
                    .GetRequiredService<ITimeLogAutomaticallyAcceptedEventHandler>();
                await handler.HandleAsync(msg, ct);
            };
        });

        services.AddOutbox(configuration);

        return services;
    }
}