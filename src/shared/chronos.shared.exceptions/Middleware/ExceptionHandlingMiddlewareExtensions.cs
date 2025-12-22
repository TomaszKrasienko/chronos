using Microsoft.AspNetCore.Builder;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseChronosExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<chronos.shared.exceptions.Middleware.ExceptionHandlingMiddleware>();
    }
}
