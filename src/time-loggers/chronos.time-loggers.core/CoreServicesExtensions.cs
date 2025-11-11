using chronos.time_loggers.core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
public static class CoreServicesExtensions
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
            IConfiguration configuration)
        => services
            .AddDal(configuration)
            .AddScoped<ITimeLoggerService, TimeLoggerService>();

    internal static T GetOptions<T>(this IServiceCollection services) where T : class
    {
        var sp = services.BuildServiceProvider();
        return sp.GetRequiredService<IOptions<T>>().Value;
    }
}
