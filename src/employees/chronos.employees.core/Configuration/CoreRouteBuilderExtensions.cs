
// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

public static class CoreRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapCore(this IEndpointRouteBuilder endpoints)
        => endpoints.MapGrpcCommunication();
}