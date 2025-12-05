using chronos.employees.core.Communication.Sync.Grpc;
using chronos.employees.core.Communication.Sync.Grpc.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

internal static class CommunicationGrpcRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGrpcCommunication(
        this IEndpointRouteBuilder app)
    {
        var options = app.ServiceProvider.GetRequiredService<IOptions<GrpcCommunicationOptions>>().Value;

        if (options.Enabled)
        {
            app.MapGrpcService<EmployeesService>();
        }

        return app;
    }
}