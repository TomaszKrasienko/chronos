using System.Net;

namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Exception thrown when a domain business rule is violated.
/// </summary>
public sealed class DomainException(
    string code,
    string[]? @params = null) : ChronosException(code, @params)
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
}
