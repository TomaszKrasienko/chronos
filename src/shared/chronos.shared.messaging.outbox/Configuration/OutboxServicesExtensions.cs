using chronos.shared.messaging;
using chronos.shared.messaging.outbox;
using chronos.shared.messaging.outbox.Configuration;
using chronos.shared.messaging.outbox.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class OutboxServicesExtensions
{
    public static IServiceCollection AddOutbox(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions(configuration);
        var options = services.GetOptions<OutboxOptions>();
        
        services
            .AddContext()
            .AddScoped<IMessageDispatcher, OutboxMessageDispatcher>()
            .AddHostedService<OutboxProcessor>();
        
        services
            .AddHostedService<OutboxMigrator>();
        
        return services;
    }

    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection(nameof(OutboxOptions)));
        return services;
    }

    private static IServiceCollection AddContext(this IServiceCollection services)
    {
        var options = services.GetOptions<OutboxOptions>();
        services.AddDbContext<OutboxDbContext>(opt => opt.UseNpgsql(options.ConnectionString));
        return services;
    }
}
