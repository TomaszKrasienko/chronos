namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Exception thrown when a domain business rule is violated.
/// </summary>
public sealed class DomainException(string code) : ChronosException
{
    public override string Code { get; } = code;
}
