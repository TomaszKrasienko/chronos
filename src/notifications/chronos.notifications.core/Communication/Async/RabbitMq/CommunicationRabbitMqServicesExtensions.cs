using chronos.notifications.core.Configuration;
using chronos.notifications.core.Events;
using chronos.notifications.core.Services;
using chronos.notifications.core.Services.NotificationSenders;
using chronos.shared.messaging.rabbit_mq.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class CommunicationRabbitMqServicesExtensions
{
    internal static IServiceCollection AddRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var appOptions = services
            .GetOptions<AppOptions>();

        // Force assembly load to ensure types are discoverable in MessagesRouteRegistry
        _ = typeof(EmployeeCreated);
        _ = typeof(SupervisorAssigned);
        _ = typeof(TimeLogCreated);

        services.AddChronosRabbitMq(
            configuration,
            appOptions.Name);
        
        services.AddConsumer<EmployeeCreated>(sp =>
        {
            return (async (msg, ct, mt) =>
            {
                using var scope = sp.CreateScope();
                var cacheService = scope.ServiceProvider.GetRequiredService<IContactsServices>();
                await cacheService.CreateAsync(
                    msg.Id,
                    msg.Email,
                    msg.SupervisorId,
                    ct);
            });
        });
        
        services.AddConsumer<SupervisorAssigned>(sp =>
        {
            return (async (msg, ct, mt) =>
            {
                using var scope = sp.CreateScope();
                var contactsService = scope.ServiceProvider.GetRequiredService<IContactsServices>();

                await contactsService.UpdateAsync(
                    msg.EmployeeId,
                    msg.SupervisorId,
                    ct);
            });
        });
        
        services.AddConsumer<TimeLogCreated>(sp =>
        {
            return (async (msg, ct, mt) =>
            {
                using var scope = sp.CreateScope();
                var timeLoggerSender = scope.ServiceProvider.GetRequiredService<INotificationSender<TimeLogCreated>>();

                await timeLoggerSender.SendAsync(
                    msg,
                    ct);
            });
        });
        
        return services;
    }
}