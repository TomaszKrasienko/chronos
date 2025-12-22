using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

public sealed class GrpcServiceUnauthorizedException(string message)
    : ChronosException(
        "grpc.unauthorized",
        message,
        null,
        HttpStatusCode.Unauthorized);
