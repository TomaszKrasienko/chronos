using System.Net;
using chronos.shared.exceptions;

namespace chronos.time_logs.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceTimeoutException(string message)
    : ChronosException(
        "http.timeout",
        message,
        null,
        HttpStatusCode.RequestTimeout);
