using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain.Events;
using chronos.time_logs.core.Domain.Rules;
using chronos.time_logs.core.Domain.ValueObjects;

namespace chronos.time_logs.core.Domain;

/// <summary>
/// Aggregate root representing a monthly time report for an employee.
/// </summary>
public sealed class MonthlyTimeReport : AggregateRoot<MonthlyTimeReportId>
{
    private readonly List<WaitingForAcceptationTimeLog> _pendingTimeLogs = [];
    private readonly List<AcceptedTimeLog> _acceptedTimeLogs = [];
    private readonly List<RejectedTimeLog> _rejectedTimeLogs = [];

    /// <summary>
    /// Gets the employee identifier.
    /// </summary>
    public EmployeeId EmployeeId { get; private set; }

    /// <summary>
    /// Gets the report period.
    /// </summary>
    public ReportPeriod Period { get; private set; }

    /// <summary>
    /// Gets the pending time logs.
    /// </summary>
    public IReadOnlyCollection<WaitingForAcceptationTimeLog> PendingTimeLogs => _pendingTimeLogs.AsReadOnly();

    /// <summary>
    /// Gets the accepted time logs.
    /// </summary>
    public IReadOnlyCollection<AcceptedTimeLog> AcceptedTimeLogs => _acceptedTimeLogs.AsReadOnly();

    /// <summary>
    /// Gets the rejected time logs.
    /// </summary>
    public IReadOnlyCollection<RejectedTimeLog> RejectedTimeLogs => _rejectedTimeLogs.AsReadOnly();

    /// <summary>
    /// Gets all time logs in this report (combined from all states).
    /// </summary>
    public IReadOnlyCollection<TimeLog> TimeLogs =>
        _pendingTimeLogs.Cast<TimeLog>()
            .Concat(_acceptedTimeLogs)
            .Concat(_rejectedTimeLogs)
            .ToList()
            .AsReadOnly();

#pragma warning disable CS8618
    private MonthlyTimeReport()
    {
    }
#pragma warning restore CS8618

    private MonthlyTimeReport(
        MonthlyTimeReportId id,
        EmployeeId employeeId,
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
    public static MonthlyTimeReport Create(
        EmployeeId employeeId,
        ReportPeriod period)
        => new(MonthlyTimeReportId.New(), employeeId, period);

    /// <summary>
    /// Adds a new time log to the report.
    /// </summary>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="time">The logged time.</param>
    /// <param name="topic">The topic.</param>
    /// <param name="notes">Optional notes.</param>
    /// <param name="supervisorId">The supervisor identifier.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The created time log identifier.</returns>
    public TimeLogId AddTimeLog(
        ContractId contractId,
        LoggedTime time,
        string topic,
        string? notes,
        EmployeeId supervisorId,
        TimeProvider timeProvider)
    {
        var timeLog = WaitingForAcceptationTimeLog.Create(
            EmployeeId,
            contractId,
            time,
            topic,
            notes,
            supervisorId,
            timeProvider);
        _pendingTimeLogs.Add(timeLog);

        AddDomainEvent(new TimeLogWaitingForAcceptationCreatedEvent(
            timeLog.Id,
            contractId,
            EmployeeId,
            time.Value,
            topic,
            notes,
            supervisorId));

        return timeLog.Id;
    }

    /// <summary>
    /// Accepts a time log.
    /// </summary>
    /// <param name="timeLogId">The time log identifier.</param>
    /// <param name="supervisorId">The supervisor identifier.</param>
    /// <param name="timeProvider">The time provider.</param>
    public void AcceptTimeLog(TimeLogId timeLogId, Ulid supervisorId, TimeProvider timeProvider)
    {
        CheckRule(new TimeLogMustExistInPendingRule(PendingTimeLogs, timeLogId));

        var pending = _pendingTimeLogs.Single(t => t.Id == timeLogId);
        var accepted = pending.Accept(supervisorId, timeProvider);

        _pendingTimeLogs.Remove(pending);
        _acceptedTimeLogs.Add(accepted);
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
        CheckRule(new TimeLogMustExistInPendingRule(PendingTimeLogs, timeLogId));

        var pending = _pendingTimeLogs.Single(t => t.Id == timeLogId);
        var rejected = pending.Reject(supervisorId, reason, timeProvider);

        _pendingTimeLogs.Remove(pending);
        _rejectedTimeLogs.Add(rejected);
    }
}
