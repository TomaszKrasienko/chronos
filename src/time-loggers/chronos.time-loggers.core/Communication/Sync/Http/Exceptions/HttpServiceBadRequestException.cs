using chronos.time_loggers.core.Exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceBadRequestException(string code)
    : ChronosException(code);
