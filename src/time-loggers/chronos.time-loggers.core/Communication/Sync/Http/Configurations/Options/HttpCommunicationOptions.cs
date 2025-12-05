namespace chronos.time_loggers.core.Communication.Sync.Http.Configurations.Options;

internal sealed record HttpCommunicationOptions
{
    public bool Enabled { get; init; }
    public required IReadOnlyCollection<HttpClientOptions> Clients { get; init; }
}