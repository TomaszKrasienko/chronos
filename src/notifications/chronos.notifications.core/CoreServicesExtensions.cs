using chronos.notifications.core.Configuration;
using chronos.notifications.core.DAL;
using chronos.notifications.core.Events.External;
using chronos.shared.configuration.Options;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CoreServicesExtensions
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .Configure<AppOptions>(configuration.GetSection(nameof(AppOptions)))
            .AddHostedService<BannerService>()
            .AddSingleton(TimeProvider.System)
            .AddDal(configuration)
            .AddRabbitMq(configuration)
            .AddScoped<IContactsRepository, ContactsRepository>()
            .AddScoped<IEmployeeCreatedEventHandler, EmployeeCreatedEventHandler>();
}
