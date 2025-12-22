// RabbitMq configuration is now in DI namespace
using chronos.employees.core.Communication.Sync.Grpc.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class CommunicationConfigurationExtensions
{
    internal static IServiceCollection AddCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddGrpcCommunication(configuration)
            .AddRabbitMq(configuration);
}