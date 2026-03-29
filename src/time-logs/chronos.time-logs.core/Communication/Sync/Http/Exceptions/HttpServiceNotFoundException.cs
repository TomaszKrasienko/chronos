using System.Net;
using chronos.shared.kernel.Exceptions;

namespace chronos.time_logs.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceNotFoundException(
    string[]? @params = null)
    : ChronosException("http_not_found", @params)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}
