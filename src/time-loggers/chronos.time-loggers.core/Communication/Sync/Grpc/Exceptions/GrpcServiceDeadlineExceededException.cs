using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

public sealed class GrpcServiceDeadlineExceededException(string message)
    : ChronosException(
        "grpc.deadline_exceeded",
        message,
        null,
        HttpStatusCode.GatewayTimeout);
