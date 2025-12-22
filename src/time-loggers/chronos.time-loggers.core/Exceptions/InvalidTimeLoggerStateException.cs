using chronos.shared.exceptions;
using System.Net;
using chronos.time_loggers.core.Domain;

namespace chronos.time_loggers.core.Exceptions;

public sealed class InvalidTimeLoggerStateException(Ulid timeLoggerId, TimeLoggerStatus currentStatus, string operation)
    : ChronosException(
        "time_logger.invalid_state",
        $"Cannot {operation} time logger {timeLoggerId} with status {currentStatus}",
        timeLoggerId.ToString(),
        HttpStatusCode.BadRequest);
