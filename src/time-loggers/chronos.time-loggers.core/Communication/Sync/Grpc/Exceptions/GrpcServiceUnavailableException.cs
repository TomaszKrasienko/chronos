using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

public sealed class GrpcServiceUnavailableException(string message)
    : ChronosException(
        "grpc.service_unavailable",
        message,
        null,
        HttpStatusCode.ServiceUnavailable);
