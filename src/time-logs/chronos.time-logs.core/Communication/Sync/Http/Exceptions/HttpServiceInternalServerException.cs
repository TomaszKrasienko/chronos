using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_logs.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceInternalServerException(string message)
    : ChronosException(
        "http.internal_server_error",
        message,
        null,
        HttpStatusCode.InternalServerError);
