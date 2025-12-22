using chronos.shared.exceptions;
using System.Net;

namespace chronos.time_loggers.core.Exceptions;

public sealed class TimeLoggerNotFoundException(Ulid timeLoggerId)
    : ChronosException(
        "time_logger.not_found",
        $"Time logger with ID {timeLoggerId} not found",
        timeLoggerId.ToString(),
        HttpStatusCode.NotFound);
