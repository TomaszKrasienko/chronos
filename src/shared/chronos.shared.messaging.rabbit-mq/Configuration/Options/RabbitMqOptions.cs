namespace chronos.shared.messaging.rabbit_mq.Configuration.Options;

public sealed record RabbitMqOptions
{
    public required string HostName { get; init; }
    public int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public string VirtualHost { get; init; } = "/";
    public Dictionary<string, MessageRouteOptions> Routes { get; init; } = [];
}