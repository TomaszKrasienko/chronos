namespace chronos.time_reports.core.Configuration;

public sealed record SavingOptions
{
    public required string Path { get; init; }
}
