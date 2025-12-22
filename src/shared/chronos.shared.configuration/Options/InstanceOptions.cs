namespace chronos.shared.configuration.Options;

public sealed record InstanceOptions
{
    public Guid Id { get; } = Guid.NewGuid();
}