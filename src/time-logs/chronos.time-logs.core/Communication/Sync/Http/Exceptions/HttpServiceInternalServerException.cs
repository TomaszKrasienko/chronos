using System.Net;
using chronos.shared.kernel.Exceptions;

namespace chronos.time_logs.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceInternalServerException(
    string[]? @params = null)
    : ChronosException("http_internal_server_error", @params)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
}
