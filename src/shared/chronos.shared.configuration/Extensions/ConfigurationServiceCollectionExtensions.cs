using chronos.shared.configuration.Options;
using chronos.shared.configuration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace chronos.shared.configuration.Extensions;

public static class ConfigurationServiceCollectionExtensions
{
    public static IServiceCollection AddBanner(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .Configure<AppOptions>(configuration.GetSection(nameof(AppOptions)))
            .AddHostedService<BannerService>();
}
