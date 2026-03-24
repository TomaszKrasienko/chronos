using System.Net;
using Microsoft.Extensions.Logging;

namespace chronos.time_logs.core.Communication.Sync.Http.Exceptions;

internal static class HttpExceptionHandler
{
    internal static void HandleException<TClass>(
        HttpResponseMessage response,
        ILogger<TClass> logger) where TClass : class
    {
        var errorCode = $"{typeof(TClass).Name}.{response.StatusCode}";

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                logger.LogWarning("Resource not found in HTTP service. Status: {StatusCode}", response.StatusCode);
                throw new HttpServiceNotFoundException(errorCode);

            case HttpStatusCode.BadRequest:
                logger.LogWarning("Bad request to HTTP service. Status: {StatusCode}", response.StatusCode);
                throw new HttpServiceBadRequestException(errorCode);

            case HttpStatusCode.RequestTimeout:
                logger.LogError("Request timeout for HTTP service. Status: {StatusCode}", response.StatusCode);
                throw new HttpServiceTimeoutException(errorCode);

            case HttpStatusCode.ServiceUnavailable:
                logger.LogError("HTTP service is unavailable. Status: {StatusCode}", response.StatusCode);
                throw new HttpServiceUnavailableException(errorCode);

            case HttpStatusCode.InternalServerError:
                logger.LogError("Internal server error in HTTP service. Status: {StatusCode}", response.StatusCode);
                throw new HttpServiceInternalServerException(errorCode);

            default:
                logger.LogError("Unknown error occurred in HTTP service. Status: {StatusCode}", response.StatusCode);
                throw new HttpServiceUnknownException($"{typeof(TClass).Name}.UnknownError");
        }
    }
}
