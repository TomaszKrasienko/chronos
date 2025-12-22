using System.Net;

namespace chronos.shared.exceptions;

public abstract class ChronosException(
    string code,
    string message,
    string? identifier,
    HttpStatusCode statusCode) : Exception(message)
{
    public string Code { get; } = code;
    public string? Identifier { get; } = identifier;
    public HttpStatusCode StatusCode { get; } = statusCode;
}
