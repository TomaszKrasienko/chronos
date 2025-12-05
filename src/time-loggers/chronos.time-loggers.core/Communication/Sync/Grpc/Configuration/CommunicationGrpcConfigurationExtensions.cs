using chronos.employees;
using chronos.shared.configuration.Extensions;
using chronos.time_loggers.core.Communication.Sync;
using chronos.time_loggers.core.Communication.Sync.Grpc;
using chronos.time_loggers.core.Communication.Sync.Grpc.Configuration.Options;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CommunicationGrpcConfigurationExtensions
{
    internal static IServiceCollection AddGrpcCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        services.AddOptions(configuration);

        var options = services.GetOptions<GrpcCommunicationOptions>();

        if (options.Enabled)
        {
            var employeesOptions = options
                .Clients
                .Single(x => x.Name == nameof(EmployeeService.EmployeeServiceClient));

            services.AddGrpcClient<EmployeeService.EmployeeServiceClient>(cfg =>
            {
                cfg.Address = new Uri(employeesOptions.Uri);
            });

            services.AddTransient<IEmployeesClient, GrpcEmployeesClient>();
        }

        return services;
    }

    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.Configure<GrpcCommunicationOptions>(configuration.GetSection(nameof(GrpcCommunicationOptions)));
}