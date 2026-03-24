using chronos.shared.kernel.Identifiers;

namespace chronos.time_logs.core.Domain;

/// <summary>
/// Time log that has been rejected by a supervisor.
/// </summary>
public sealed class RejectedTimeLog : TimeLog
{
    /// <summary>
    /// Gets the rejection reason.
    /// </summary>
    public string Reason { get; private set; }

    /// <summary>
    /// Gets the identifier of the supervisor who rejected the time log.
    /// </summary>
    public Ulid RejectedBy { get; private set; }

    /// <summary>
    /// Gets the timestamp when the time log was rejected.
    /// </summary>
    public DateTime RejectedAt { get; private set; }

#pragma warning disable CS8618
    private RejectedTimeLog()
    {
    }
#pragma warning restore CS8618

    private RejectedTimeLog(
        TimeLogId id,
        Ulid employeeId,
        Ulid contractId,
        ValueObjects.LoggedHours hours,
        string topic,
        string? notes,
        DateTime createdAt,
        string reason,
        Ulid rejectedBy,
        DateTime rejectedAt) : base(id, employeeId, contractId, hours, topic, notes, createdAt)
    {
        Reason = reason;
        RejectedBy = rejectedBy;
        RejectedAt = rejectedAt;
    }

    /// <summary>
    /// Creates a rejected time log from a pending time log.
    /// </summary>
    /// <param name="pending">The pending time log.</param>
    /// <param name="rejectedBy">The supervisor who rejected.</param>
    /// <param name="reason">The rejection reason.</param>
    /// <param name="timeProvider">The time provider.</param>
    internal static RejectedTimeLog FromPending(
        WaitingForAcceptation pending,
        Ulid rejectedBy,
        string reason,
        TimeProvider timeProvider)
        => new(
            pending.Id,
            pending.EmployeeId,
            pending.ContractId,
            pending.Hours,
            pending.Topic,
            pending.Notes,
            pending.CreatedAt,
            reason,
            rejectedBy,
            timeProvider.GetUtcNow().UtcDateTime);
}
