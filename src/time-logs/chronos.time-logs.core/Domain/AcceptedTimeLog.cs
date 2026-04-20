using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain.ValueObjects;

namespace chronos.time_logs.core.Domain;

/// <summary>
/// Time log that has been accepted by a supervisor.
/// </summary>
public sealed class AcceptedTimeLog : TimeLog
{
    public const string StatusName = "Accepted";

    /// <summary>
    /// Gets the identifier of the supervisor who accepted the time log.
    /// </summary>
    public Ulid AcceptedBy { get; private set; }

    /// <summary>
    /// Gets the timestamp when the time log was accepted.
    /// </summary>
    public DateTime AcceptedAt { get; private set; }

    private AcceptedTimeLog()
    {
    }

    private AcceptedTimeLog(
        TimeLogId id,
        EmployeeId employeeId,
        ContractId contractId,
        LoggedTime time,
        string topic,
        string? notes,
        DateTime createdAt,
        Ulid acceptedBy,
        DateTime acceptedAt) : base(id, employeeId, contractId, time, topic, notes, createdAt)
    {
        AcceptedBy = acceptedBy;
        AcceptedAt = acceptedAt;
    }

    /// <summary>
    /// Creates an accepted time log from a pending time log.
    /// </summary>
    /// <param name="pending">The pending time log.</param>
    /// <param name="acceptedBy">The supervisor who accepted.</param>
    /// <param name="timeProvider">The time provider.</param>
    internal static AcceptedTimeLog FromPending(
        WaitingForAcceptationTimeLog pending,
        Ulid acceptedBy,
        TimeProvider timeProvider)
        => new(
            pending.Id,
            pending.EmployeeId,
            pending.ContractId,
            pending.Time,
            pending.Topic,
            pending.Notes,
            pending.CreatedAt,
            acceptedBy,
            timeProvider.GetUtcNow().UtcDateTime);
}
