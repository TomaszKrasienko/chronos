using chronos.employees.core.Communication.Sync.Grpc.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace chronos.employees.core.Communication.Configuration;

internal static class CommunicationConfigurationExtensions
{
    internal static IServiceCollection AddCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddGrpcCommunication(configuration);
}