using System.Net;

namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Exception thrown when communication with an external service fails unexpectedly.
/// </summary>
public sealed class InvalidCommunicationException(
    string code,
    string[]? @params = null) : ChronosException(code, @params)
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
}
