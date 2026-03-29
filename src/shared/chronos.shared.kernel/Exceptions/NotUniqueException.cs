using System.Net;

namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Exception thrown when a uniqueness constraint is violated.
/// </summary>
public sealed class NotUniqueException(
    string entityName,
    string[]? @params = null) : ChronosException($"{entityName}_is_not_unique", @params)
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
}
