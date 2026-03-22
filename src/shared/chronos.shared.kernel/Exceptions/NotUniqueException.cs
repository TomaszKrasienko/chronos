namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Exception thrown when a uniqueness constraint is violated.
/// </summary>
public sealed class NotUniqueException(string entityName, string paramName, object paramValue) : ChronosException
{
    private const string ErrorCode = "not_unique";

    public override string Code => ErrorCode;

    public string EntityName { get; } = entityName;
    public string ParamName { get; } = paramName;
    public object ParamValue { get; } = paramValue;
}
