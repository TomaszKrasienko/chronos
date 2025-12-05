namespace chronos.time_loggers.core.Communication.Sync.Grpc.Configuration.Options;

public sealed record GrpcClientOptions
{
    public required string Name { get; init; }
    
    public required string Uri { get; init; }
}