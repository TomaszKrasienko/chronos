using chronos.time_logs.core.Events.External;
using chronos.time_logs.core.Services;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CoreServicesExtensions
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddSingleton(TimeProvider.System)
            .AddDal(configuration)
            .AddCommunication(configuration)
            .AddRabbitMq(configuration)
            .AddScoped<IReadTimeReportService, TimeReportService>()
            .AddScoped<IWriteTimeReportService, TimeReportService>()
            .AddScoped<ITimeLogAutomaticallyAcceptedEventHandler, TimeLogAutomaticallyAcceptedEventHandler>();
}
