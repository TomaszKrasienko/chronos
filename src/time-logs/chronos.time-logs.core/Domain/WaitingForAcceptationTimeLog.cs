using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain.ValueObjects;

namespace chronos.time_logs.core.Domain;

/// <summary>
/// Time log waiting for supervisor acceptance.
/// </summary>
public sealed class WaitingForAcceptationTimeLog : TimeLog
{
    public const string StatusName = "WaitingForAcceptation";

    /// <summary>
    /// Gets the supervisor identifier who should accept/reject the time log.
    /// </summary>
    public EmployeeId SupervisorId { get; private set; }

    private WaitingForAcceptationTimeLog()
    {
    }

    private WaitingForAcceptationTimeLog(
        TimeLogId id,
        EmployeeId employeeId,
        ContractId contractId,
        LoggedTime time,
        string topic,
        string? notes,
        DateTime createdAt,
        EmployeeId supervisorId) : base(id, employeeId, contractId, time, topic, notes, createdAt)
    {
        SupervisorId = supervisorId;
    }

    /// <summary>
    /// Creates a new time log waiting for acceptation.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="time">The logged time.</param>
    /// <param name="topic">The topic.</param>
    /// <param name="notes">Optional notes.</param>
    /// <param name="supervisorId">The supervisor identifier.</param>
    /// <param name="timeProvider">The time provider.</param>
    internal static WaitingForAcceptationTimeLog Create(
        EmployeeId employeeId,
        ContractId contractId,
        LoggedTime time,
        string topic,
        string? notes,
        EmployeeId supervisorId,
        TimeProvider timeProvider)
        => new(
            TimeLogId.New(),
            employeeId,
            contractId,
            time,
            topic,
            notes,
            timeProvider.GetUtcNow().UtcDateTime,
            supervisorId);

    /// <summary>
    /// Accepts this time log.
    /// </summary>
    /// <param name="acceptedBy">The supervisor who accepted.</param>
    /// <param name="timeProvider">The time provider.</param>
    internal AcceptedTimeLog Accept(Ulid acceptedBy, TimeProvider timeProvider)
        => AcceptedTimeLog.FromPending(this, acceptedBy, timeProvider);

    /// <summary>
    /// Rejects this time log.
    /// </summary>
    /// <param name="rejectedBy">The supervisor who rejected.</param>
    /// <param name="reason">The rejection reason.</param>
    /// <param name="timeProvider">The time provider.</param>
    internal RejectedTimeLog Reject(Ulid rejectedBy, string reason, TimeProvider timeProvider)
        => RejectedTimeLog.FromPending(this, rejectedBy, reason, timeProvider);
}
