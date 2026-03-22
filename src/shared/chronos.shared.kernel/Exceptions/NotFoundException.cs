namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Exception thrown when a requested entity is not found.
/// </summary>
public sealed class NotFoundException(string entityName, object id) : ChronosException
{
    private const string ErrorCode = "not_found";

    public override string Code => ErrorCode;

    public string EntityName { get; } = entityName;
    public object Id { get; } = id;
}
