using chronos.shared.configuration.Options;
using chronos.shared.configuration.Services;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigurationServiceCollectionExtensions
{
    public static IServiceCollection AddBanner(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .Configure<AppOptions>(configuration.GetSection(nameof(AppOptions)))
            .AddSingleton<InstanceOptions>()
            .AddHostedService<BannerService>();
}
