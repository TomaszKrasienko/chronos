using chronos.shared.messaging.rabbit_mq.Configuration;
using chronos.time_reports.core.Configuration;
using chronos.time_reports.core.Events;
using chronos.time_reports.core.Services;
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
        _ = typeof(TimeLogAccepted);
        _ = typeof(TimeLogRejected);

        services.AddChronosRabbitMq(
            configuration,
            appOptions.Name);

        services.AddConsumer<TimeLogAccepted>(sp =>
        {
            return async (msg, ct, mt) =>
            {
                using var scope = sp.CreateScope();
                var timeReportsService = scope.ServiceProvider.GetRequiredService<ITimeReportsService>();
                await timeReportsService.UpsertTimeReportAsync(
                    msg.Id,
                    msg.EmployeeId,
                    msg.TimeSpan,
                    true);
            };
        });

        services.AddConsumer<TimeLogRejected>(sp =>
        {
            return async (msg, ct, mt) =>
            {
                using var scope = sp.CreateScope();
                var timeReportsService = scope.ServiceProvider.GetRequiredService<ITimeReportsService>();
                await timeReportsService.UpsertTimeReportAsync(
                    msg.Id,
                    msg.EmployeeId,
                    msg.TimeSpan,
                    false);
            };
        });

        return services;
    }
}
