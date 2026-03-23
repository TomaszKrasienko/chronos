using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain.Rules;
using chronos.time_logs.core.Domain.ValueObjects;

namespace chronos.time_logs.core.Domain;

/// <summary>
/// Aggregate root representing a monthly time report for an employee.
/// </summary>
public sealed class MonthlyTimeReport : AggregateRoot<MonthlyTimeReportId>
{
    private readonly List<TimeLog> _timeLogs = [];

    /// <summary>
    /// Gets the employee identifier.
    /// </summary>
    public Ulid EmployeeId { get; private set; }

    /// <summary>
    /// Gets the report period.
    /// </summary>
    public ReportPeriod Period { get; private set; }

    /// <summary>
    /// Gets the time logs in this report.
    /// </summary>
    public IReadOnlyCollection<TimeLog> TimeLogs => _timeLogs.AsReadOnly();

#pragma warning disable CS8618
    private MonthlyTimeReport()
    {
    }
#pragma warning restore CS8618

    private MonthlyTimeReport(
        MonthlyTimeReportId id,
        Ulid employeeId,
        ReportPeriod period) : base(id)
    {
        EmployeeId = employeeId;
        Period = period;
    }

    /// <summary>
    /// Creates a new monthly time report.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="period">The report period.</param>
    public static MonthlyTimeReport Create(Ulid employeeId, ReportPeriod period)
        => new(MonthlyTimeReportId.New(), employeeId, period);

    /// <summary>
    /// Adds a new time log to the report.
    /// </summary>
    /// <param name="timeLog">The time log to add.</param>
    public void AddTimeLog(WaitingForAcceptation timeLog)
    {
        _timeLogs.Add(timeLog);
    }

    /// <summary>
    /// Accepts a time log.
    /// </summary>
    /// <param name="timeLogId">The time log identifier.</param>
    /// <param name="supervisorId">The supervisor identifier.</param>
    /// <param name="timeProvider">The time provider.</param>
    public void AcceptTimeLog(TimeLogId timeLogId, Ulid supervisorId, TimeProvider timeProvider)
    {
        CheckRule(new TimeLogMustExistInReportRule(TimeLogs, timeLogId));
        CheckRule(new TimeLogMustBePendingRule(TimeLogs, timeLogId));

        var pending = (WaitingForAcceptation)_timeLogs.Single(t => t.Id == timeLogId);
        var accepted = pending.Accept(supervisorId, timeProvider);

        _timeLogs.Remove(pending);
        _timeLogs.Add(accepted);
    }

    /// <summary>
    /// Rejects a time log.
    /// </summary>
    /// <param name="timeLogId">The time log identifier.</param>
    /// <param name="supervisorId">The supervisor identifier.</param>
    /// <param name="reason">The rejection reason.</param>
    /// <param name="timeProvider">The time provider.</param>
    public void RejectTimeLog(TimeLogId timeLogId, Ulid supervisorId, string reason, TimeProvider timeProvider)
    {
        CheckRule(new TimeLogMustExistInReportRule(TimeLogs, timeLogId));
        CheckRule(new TimeLogMustBePendingRule(TimeLogs, timeLogId));

        var pending = (WaitingForAcceptation)_timeLogs.Single(t => t.Id == timeLogId);
        var rejected = pending.Reject(supervisorId, reason, timeProvider);

        _timeLogs.Remove(pending);
        _timeLogs.Add(rejected);
    }
}
