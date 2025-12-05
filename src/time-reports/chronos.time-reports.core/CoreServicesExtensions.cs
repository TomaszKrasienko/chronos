using chronos.time_reports.core.Configuration;
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
            .AddDal(configuration)
            .Configure<AppOptions>(configuration.GetSection(nameof(AppOptions)))
            .AddHostedService<BannerService>();

    internal static T GetOptions<T>(this IServiceCollection services) where T : class
    {
        var sp = services.BuildServiceProvider();
        return sp.GetRequiredService<IOptions<T>>().Value;
    }
}
