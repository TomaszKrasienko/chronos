using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_logs.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceUnavailableException(string message)
    : ChronosException(
        "http.service_unavailable",
        message,
        null,
        HttpStatusCode.ServiceUnavailable);
