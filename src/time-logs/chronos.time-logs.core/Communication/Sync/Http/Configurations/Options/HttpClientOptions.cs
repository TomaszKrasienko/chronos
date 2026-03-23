namespace chronos.time_logs.core.Communication.Sync.Http.Configurations.Options;

public sealed record HttpClientOptions
{
    public required string Name { get; init; }
    public required string Uri { get; init; }
    public string ResiliencePattern { get; init; } = "Linear";
    public int Attempts { get; init; } = 3;
    public TimeSpan TimeSpan { get; init; } = TimeSpan.FromSeconds(10);
}