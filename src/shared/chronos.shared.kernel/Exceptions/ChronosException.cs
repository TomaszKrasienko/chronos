namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Base exception for all Chronos domain exceptions.
/// </summary>
public abstract class ChronosException : Exception
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    public abstract string Code { get; }

    protected ChronosException()
    {
    }
}
