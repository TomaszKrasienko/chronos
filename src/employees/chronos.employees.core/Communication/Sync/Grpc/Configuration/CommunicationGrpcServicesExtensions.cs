using chronos.employees.core.Communication.Sync.Grpc;
using chronos.employees.core.Communication.Sync.Grpc.Configuration;
using chronos.employees.core.Communication.Sync.Grpc.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CommunicationGrpcServicesExtensions
{
    internal static IServiceCollection AddGrpcCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions(configuration);

        var options = services.GetOptions<GrpcCommunicationOptions>();

        if (options.Enabled)
        {
            services.AddGrpc(grpcOptions =>
            {
                grpcOptions.Interceptors.Add<ExceptionInterceptor>();
            });
        }

        return services;
    }

    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.Configure<GrpcCommunicationOptions>(configuration.GetSection(nameof(GrpcCommunicationOptions)));
}