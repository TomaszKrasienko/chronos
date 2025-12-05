using chronos.time_loggers.core.Exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceTimeoutException(string code)
    : ChronosException(code);
