using chronos.time_loggers.core.Communication.Sync.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace chronos.time_loggers.core.Communication;

internal static class CommunicationConfigurationExtensions
{
    internal static IServiceCollection AddCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddSyncCommunication(configuration);
}