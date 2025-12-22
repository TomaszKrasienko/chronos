using chronos.time_loggers.core.Communication;
using chronos.time_loggers.core.Services;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CoreServicesExtensions
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
            IConfiguration configuration)
        => services
            .AddDal(configuration)
            .AddCommunication(configuration)
            .AddScoped<ITimeLoggerService, TimeLoggerService>()
            .AddRabbitMq(configuration)
            .AddBanner(configuration);
}
