using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_logs.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceUnknownException(string message)
    : ChronosException(
        "http.unknown",
        message,
        null,
        HttpStatusCode.InternalServerError);
