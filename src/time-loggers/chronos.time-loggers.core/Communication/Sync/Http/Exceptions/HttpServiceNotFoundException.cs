using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceNotFoundException(string message)
    : ChronosException(
        "http.not_found",
        message,
        null,
        HttpStatusCode.NotFound);
