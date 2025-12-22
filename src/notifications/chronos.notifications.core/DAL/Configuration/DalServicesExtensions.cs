using chronos.notifications.core.DAL;
using chronos.notifications.core.DAL.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesExtensions
{
    internal static IServiceCollection AddDal(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddContext();

    private static IServiceCollection AddContext(
        this IServiceCollection services)
    {
        var mongoOptions = services.GetOptions<DalOptions>();
        services.AddDbContext<NotificationsDbContext>(options => options.UseMongoDB(
            mongoOptions.ConnectionString,
            mongoOptions.DatabaseName));
        return services;
    }

    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DalOptions>(configuration.GetSection(nameof(DalOptions)));
        return services;
    }
}
