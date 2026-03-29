using System.Net;
using chronos.shared.kernel.Exceptions;

namespace chronos.time_logs.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceTimeoutException(
    string[]? @params = null)
    : ChronosException("http_timeout", @params)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.RequestTimeout;
}
