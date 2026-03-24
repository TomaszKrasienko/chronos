using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class SyncCommunicationServicesExtensions
{
    internal static IServiceCollection AddSyncCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.AddHttpCommunication(configuration);
}
