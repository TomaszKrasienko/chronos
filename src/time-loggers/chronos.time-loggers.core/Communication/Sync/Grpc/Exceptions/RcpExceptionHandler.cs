using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

internal static class RcpExceptionHandler
{
    internal static void HandleException<TClass>(
        RpcException ex,
        ILogger<TClass> logger) where TClass : class
    {
        var errorCode = $"{typeof(TClass).Name}.{ex.StatusCode}";

        switch (ex.StatusCode)
        {
            case StatusCode.NotFound:
                logger.LogWarning(ex, "Resource not found in gRPC service");
                throw new GrpcServiceNotFoundException(errorCode);

            case StatusCode.InvalidArgument:
                logger.LogWarning(ex, "Invalid argument provided to gRPC service");
                throw new GrpcServiceBadRequestException(errorCode);

            case StatusCode.Unauthenticated:
                logger.LogWarning(ex, "Unauthenticated request to gRPC service");
                throw new GrpcServiceUnauthorizedException(errorCode);

            case StatusCode.PermissionDenied:
                logger.LogWarning(ex, "Permission denied for gRPC service");
                throw new GrpcServiceForbiddenException(errorCode);

            case StatusCode.DeadlineExceeded:
                logger.LogError(ex, "Deadline exceeded for gRPC service");
                throw new GrpcServiceDeadlineExceededException(errorCode);

            case StatusCode.Unavailable:
                logger.LogError(ex, "gRPC service is unavailable");
                throw new GrpcServiceUnavailableException(errorCode);

            case StatusCode.Internal:
                logger.LogError(ex, "Internal error in gRPC service");
                throw new GrpcServiceInternalServerException(errorCode);

            case StatusCode.Cancelled:
                logger.LogWarning(ex, "gRPC request was cancelled");
                throw new GrpcServiceCancelledException(errorCode);

            default:
                logger.LogError(ex, "Unknown error occurred in gRPC service. Status code: {StatusCode}", ex.StatusCode);
                throw new GrpcServiceUnknownException($"{nameof(TClass)}.UnknownError");
        }
    }
}