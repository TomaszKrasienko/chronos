using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

public sealed class GrpcServiceNotFoundException(string message)
    : ChronosException(
        "grpc.not_found",
        message,
        null,
        HttpStatusCode.NotFound);
