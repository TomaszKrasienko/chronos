using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chronos.shared.exceptions.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ChronosException ex)
        {
            await HandleChronosExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private Task HandleChronosExceptionAsync(HttpContext context, ChronosException exception)
    {
        logger.LogWarning(exception,
            "Chronos exception occurred: {Code} - {Message}",
            exception.Code,
            exception.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred",
            Status = (int)exception.StatusCode,
            Detail = exception.Message,
            Instance = context.Request.Path,
            Type = $"https://httpstatuses.com/{(int)exception.StatusCode}",
            Extensions =
            {
                ["code"] = exception.Code,
                ["identifier"] = exception.Identifier,
                ["traceId"] = context.TraceIdentifier
            }
        };

        context.Response.StatusCode = (int)exception.StatusCode;
        context.Response.ContentType = "application/problem+json";

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails, JsonOptions));
    }

    private Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception,
            "Unhandled exception occurred: {Message}",
            exception.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "An unexpected error occurred",
            Status = (int)HttpStatusCode.InternalServerError,
            Detail = "An unexpected error occurred. Please try again later.",
            Instance = context.Request.Path,
            Type = $"https://httpstatuses.com/{(int)HttpStatusCode.InternalServerError}",
            Extensions =
            {
                ["code"] = "internal_server_error",
                ["traceId"] = context.TraceIdentifier
            }
        };

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/problem+json";

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails, JsonOptions));
    }
}
