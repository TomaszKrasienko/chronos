namespace chronos.jobs.synchronizer.Jobs;

public sealed class FailureFirstJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("This job is designed to fail.");
    }
}
