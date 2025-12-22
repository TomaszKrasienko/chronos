using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

public sealed class GrpcServiceCancelledException(string message)
    : ChronosException(
        "grpc.cancelled",
        message,
        null,
        HttpStatusCode.BadRequest);
