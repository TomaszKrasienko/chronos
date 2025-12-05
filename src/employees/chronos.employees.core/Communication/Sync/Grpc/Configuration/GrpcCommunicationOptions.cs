namespace chronos.employees.core.Communication.Sync.Grpc.Configuration;

public sealed record GrpcCommunicationOptions
{
    public bool Enabled { get; init; }
}