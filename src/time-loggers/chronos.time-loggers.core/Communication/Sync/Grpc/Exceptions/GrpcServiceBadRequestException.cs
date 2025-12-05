using chronos.time_loggers.core.Exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;

public sealed class GrpcServiceBadRequestException(string code)
    : ChronosException(code);
