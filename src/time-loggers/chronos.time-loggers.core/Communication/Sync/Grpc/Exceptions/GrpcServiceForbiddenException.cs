using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

public sealed class GrpcServiceForbiddenException(string message)
    : ChronosException(
        "grpc.forbidden",
        message,
        null,
        HttpStatusCode.Forbidden);
