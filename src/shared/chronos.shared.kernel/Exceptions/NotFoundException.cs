using System.Net;

namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Exception thrown when a requested entity is not found.
/// </summary>
public sealed class NotFoundException(
    string entityName,
    string[]? @params = null) : ChronosException($"{entityName}_not_found", @params)
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;
}
