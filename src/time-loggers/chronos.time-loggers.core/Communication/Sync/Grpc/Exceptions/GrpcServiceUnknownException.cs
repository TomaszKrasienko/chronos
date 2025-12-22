using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

public sealed class GrpcServiceUnknownException(string message)
    : ChronosException(
        "grpc.unknown",
        message,
        null,
        HttpStatusCode.InternalServerError);
