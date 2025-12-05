namespace chronos.time_reports.core.DAL.Configuration;

public sealed record DalOptions
{
    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
}
