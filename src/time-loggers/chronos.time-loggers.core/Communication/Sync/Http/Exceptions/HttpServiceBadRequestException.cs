using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_loggers.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceBadRequestException(string message)
    : ChronosException(
        "http.bad_request",
        message,
        null,
        HttpStatusCode.BadRequest);
