namespace chronos.shared.messaging.outbox.Configuration;

public sealed record OutboxOptions
{
    public required string ConnectionString { get; init; }
}