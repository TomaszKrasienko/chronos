namespace chronos.shared.kernel;

/// <summary>
/// Represents a strongly-typed entity identifier.
/// </summary>
public interface IEntityId
{
    /// <summary>
    /// Gets the underlying Ulid value.
    /// </summary>
    Ulid Value { get; }
}
