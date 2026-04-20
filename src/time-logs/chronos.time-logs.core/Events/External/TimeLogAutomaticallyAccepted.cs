using chronos.shared.kernel.Identifiers;
using chronos.shared.messaging;
using chronos.time_logs.core.DAL;
using chronos.time_logs.core.Domain;

namespace chronos.time_logs.core.Events.External;

public sealed record TimeLogAutomaticallyAccepted(
    TimeLogId TimeLogId) : IMessage;

/// <summary>
/// Handler for <see cref="TimeLogAutomaticallyAccepted"/> integration event.
/// </summary>
public interface ITimeLogAutomaticallyAcceptedEventHandler
{
    Task HandleAsync(
        TimeLogAutomaticallyAccepted @event,
        CancellationToken cancellationToken = default);
}

internal sealed class TimeLogAutomaticallyAcceptedEventHandler(
    ITimeLogsRepository timeLogsRepository,
    TimeProvider timeProvider) : ITimeLogAutomaticallyAcceptedEventHandler
{
    /// <inheritdoc />
    public async Task HandleAsync(
        TimeLogAutomaticallyAccepted @event,
        CancellationToken cancellationToken = default)
    {
        var report = await timeLogsRepository.GetByTimeLogIdAsync(@event.TimeLogId, cancellationToken);
        if (report is null)
        {
            return;
        }

        var pendingTimeLog = report.PendingTimeLogs.SingleOrDefault(x => x.Id == @event.TimeLogId);
        if (pendingTimeLog is null)
        {
            return;
        }

        report.AcceptTimeLog(@event.TimeLogId, pendingTimeLog.SupervisorId.Value, timeProvider);
        await timeLogsRepository.UpdateAsync(report, cancellationToken);
    }
}
