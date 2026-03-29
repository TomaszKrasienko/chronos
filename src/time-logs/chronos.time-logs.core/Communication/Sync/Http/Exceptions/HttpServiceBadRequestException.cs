using System.Net;
using chronos.shared.kernel.Exceptions;

namespace chronos.time_logs.core.Communication.Sync.Http.Exceptions;

public sealed class HttpServiceBadRequestException(
    string[]? @params = null)
    : ChronosException("http_bad_request", @params)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
