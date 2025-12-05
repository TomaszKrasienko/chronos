using Microsoft.AspNetCore.Routing;

namespace chronos.employees.core.Communication.Configuration;

internal static class CommunicationRouteBuilderExtensions
{
    internal static IEndpointRouteBuilder MapCommunication(this IEndpointRouteBuilder builder)
        => builder.MapGrpcCommunication();
}