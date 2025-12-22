using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class SyncCommunicationServicesExtensions
{
    internal static IServiceCollection AddSyncCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ValidateOptions(configuration);
        
        return services
            .AddGrpcCommunication(configuration)
            .AddHttpCommunication(configuration);
    }

    private static void ValidateOptions(IConfiguration configuration)
    {
        var grpcCommunicationEnabled = configuration
            .GetValue<bool>("GrpcCommunicationOptions:Enabled");
        
        var httpCommunicationEnabled = configuration
            .GetValue<bool>("HttpCommunicationOptions:Enabled");

        if (!grpcCommunicationEnabled && !httpCommunicationEnabled
            || grpcCommunicationEnabled && httpCommunicationEnabled)
        {
            throw new ArgumentException("Invalid sync communication configuration");
        }
    }
}