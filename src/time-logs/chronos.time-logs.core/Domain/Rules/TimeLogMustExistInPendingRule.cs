using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.time_logs.core.Domain.Rules;

/// <summary>
/// Rule that validates time log exists in pending time logs collection.
/// </summary>
internal sealed class TimeLogMustExistInPendingRule(
    IReadOnlyCollection<WaitingForAcceptationTimeLog> pendingTimeLogs,
    TimeLogId timeLogId) : IBusinessRule
{
    public string Code => "time_log_must_exist_in_pending";

    public bool IsBroken() => !pendingTimeLogs.Any(t => t.Id == timeLogId);
}
