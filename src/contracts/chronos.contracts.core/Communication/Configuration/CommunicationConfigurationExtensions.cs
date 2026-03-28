using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class CommunicationConfigurationExtensions
{
    internal static IServiceCollection AddCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.AddRabbitMq(configuration);
}
