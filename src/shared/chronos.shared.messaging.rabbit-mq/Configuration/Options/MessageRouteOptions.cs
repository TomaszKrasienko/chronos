namespace chronos.shared.messaging.rabbit_mq.Configuration.Options;

public sealed record MessageRouteOptions
{
    public required string Exchange { get; init; }
    public string? Queue { get; init; }
    public required string[] RoutingKeys { get; init; }
    public bool IsTemporary { get; init; }
}