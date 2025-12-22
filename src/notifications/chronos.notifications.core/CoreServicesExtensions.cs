using chronos.notifications.core.Configuration;
using chronos.notifications.core.Events;
using chronos.notifications.core.Services;
using chronos.notifications.core.Services.NotificationSenders;
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
            .Configure<AppOptions>(configuration.GetSection(nameof(AppOptions)))
            .AddHostedService<BannerService>()
            .AddSingleton(TimeProvider.System)
            .AddDal(configuration)
            .AddRabbitMq(configuration)
            .AddScoped<INotificationSender<TimeLogCreated>, TimeLogCreatedNotificationSender>()
            .AddScoped<INotificationSender<EmployeeCreated>, EmployeeCreatedNotificationSender>()
            .AddSingleton<INotificationSenderFactory, NotificationSenderFactory>()
            .AddScoped<IContactsServices, ContactsService>()
            .AddScoped<INotificationMessagesService, NotificationMessagesService>();
}
