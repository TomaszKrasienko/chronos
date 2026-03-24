using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.time_logs.core.Domain.Rules;

/// <summary>
/// Rule that validates time log exists in the report.
/// </summary>
internal sealed class TimeLogMustExistInReportRule(IReadOnlyCollection<TimeLog> timeLogs, TimeLogId timeLogId) : IBusinessRule
{
    public string Code => "time_log_must_exist_in_report";

    public bool IsBroken() => !timeLogs.Any(t => t.Id == timeLogId);
}
