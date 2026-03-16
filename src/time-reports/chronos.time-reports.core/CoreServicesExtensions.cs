using chronos.time_reports.core.Configuration;
using chronos.time_reports.core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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
            .Configure<AppOptions>(configuration.GetSection(nameof(AppOptions)))
            .Configure<SavingOptions>(configuration.GetSection(nameof(SavingOptions)))
            .AddRabbitMq(configuration)
            .AddScoped<ITimeReportsService, TimeReportsService>()
            .AddHostedService<BannerService>();
}
