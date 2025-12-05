using System.Net;
using chronos.time_loggers.core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace chronos.time_loggers.api.Exceptions;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private const string UnexpectedErrorCode = "UnexpectedError";

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var problemDetails = exception switch
        {
            ChronosException chronosException => CreateProblemDetails(
                httpContext,
                HttpStatusCode.BadRequest,
                chronosException.Code,
                chronosException.Message),

            _ => CreateProblemDetails(
                httpContext,
                HttpStatusCode.InternalServerError,
                UnexpectedErrorCode,
                "An unexpected error occurred")
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext context,
        HttpStatusCode statusCode,
        string errorCode,
        string detail)
    {
        return new ProblemDetails
        {
            Status = (int)statusCode,
            Title = errorCode,
            Detail = detail,
            Instance = context.Request.Path
        };
    }
}
