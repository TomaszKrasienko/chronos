namespace chronos.shared.kernel;

/// <summary>
/// Represents a domain business rule that can be validated.
/// </summary>
public interface IBusinessRule
{
    /// <summary>
    /// Gets the error code when the rule is broken.
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Determines whether the business rule is broken.
    /// </summary>
    /// <returns>True if the rule is broken; otherwise, false.</returns>
    bool IsBroken();
}
