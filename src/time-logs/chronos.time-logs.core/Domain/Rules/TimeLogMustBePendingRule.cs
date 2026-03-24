using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.time_logs.core.Domain.Rules;

/// <summary>
/// Rule that validates time log is in pending state (WaitingForAcceptation).
/// </summary>
internal sealed class TimeLogMustBePendingRule(IReadOnlyCollection<TimeLog> timeLogs, TimeLogId timeLogId) : IBusinessRule
{
    public string Code => "time_log_must_be_pending";

    public bool IsBroken() => !timeLogs.Any(t => t.Id == timeLogId && t is WaitingForAcceptation);
}
