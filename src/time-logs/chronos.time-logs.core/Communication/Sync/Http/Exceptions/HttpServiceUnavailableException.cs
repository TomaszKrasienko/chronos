using System.Net;
using chronos.shared.kernel.Exceptions;

namespace chronos.time_logs.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceUnavailableException(
    string[]? @params = null)
    : ChronosException("http_service_unavailable", @params)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.ServiceUnavailable;
}
