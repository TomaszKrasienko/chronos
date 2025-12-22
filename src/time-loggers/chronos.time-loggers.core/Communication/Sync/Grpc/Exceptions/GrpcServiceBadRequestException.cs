using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

public sealed class GrpcServiceBadRequestException(string message)
    : ChronosException(
        "grpc.bad_request",
        message,
        null,
        HttpStatusCode.BadRequest);
