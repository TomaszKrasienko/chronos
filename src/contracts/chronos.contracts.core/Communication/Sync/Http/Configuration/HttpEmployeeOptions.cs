namespace chronos.contracts.core.Communication.Sync.Http.Configuration;

public sealed record HttpEmployeeOptions
{
    public required string Url { get; init; }
}
