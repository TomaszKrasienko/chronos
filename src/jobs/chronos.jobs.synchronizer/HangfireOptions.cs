namespace chronos.jobs.synchronizer;

public sealed class HangfireOptions
{
    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = string.Empty;
}
