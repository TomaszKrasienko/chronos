// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Http;

/// <summary>
/// Extension methods for <see cref="HttpContext"/>.
/// </summary>
public static class HttpContextExtensions
{
    private const string ResourceIdHeader = "X-Resource-Id";

    /// <summary>
    /// Adds resource ID to response headers.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="resourceId">The resource identifier to add.</param>
    public static void AddResourceId(this HttpContext context, string resourceId)
    {
        context.Response.Headers.Append(ResourceIdHeader, resourceId);
    }

    /// <summary>
    /// Adds resource ID to response headers.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="resourceId">The resource identifier to add.</param>
    public static void AddResourceId(this HttpContext context, Ulid resourceId)
    {
        context.AddResourceId(resourceId.ToString());
    }
}
