using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

public sealed class GrpcServiceInternalServerException(string message)
    : ChronosException(
        "grpc.internal_server_error",
        message,
        null,
        HttpStatusCode.InternalServerError);
