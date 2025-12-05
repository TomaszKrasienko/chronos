namespace chronos.time_loggers.core.Communication.Sync.Grpc.Configuration.Options;

public sealed record GrpcCommunicationOptions
{
    public bool Enabled { get; init; }
    public required IReadOnlyCollection<GrpcClientOptions> Clients { get; init; }
}