namespace chronos.shared.kernel.Identifiers;

/// <summary>
/// Strongly-typed identifier for the NotificationMessage entity.
/// </summary>
public readonly record struct NotificationMessageId(Ulid Value) : IEntityId
{
    public static NotificationMessageId New() => new(Ulid.NewUlid());
}
