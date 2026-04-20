using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain.ValueObjects;

namespace chronos.time_logs.core.Domain;

/// <summary>
/// Abstract base class for time log entities.
/// </summary>
public abstract class TimeLog : Entity<TimeLogId>
{
    /// <summary>
    /// Gets the employee identifier who logged the time.
    /// </summary>
    public EmployeeId EmployeeId { get; protected set; }

    /// <summary>
    /// Gets the contract identifier for which time is logged.
    /// </summary>
    public ContractId ContractId { get; protected set; }

    /// <summary>
    /// Gets the logged time.
    /// </summary>
    public LoggedTime Time { get; protected set; }

    /// <summary>
    /// Gets the topic for the time log.
    /// </summary>
    public string Topic { get; protected set; }

    /// <summary>
    /// Gets the optional notes.
    /// </summary>
    public string? Notes { get; protected set; }

    /// <summary>
    /// Gets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

#pragma warning disable CS8618
    protected TimeLog()
    {
    }
#pragma warning restore CS8618

    protected TimeLog(
        TimeLogId id,
        EmployeeId employeeId,
        ContractId contractId,
        LoggedTime time,
        string topic,
        string? notes,
        DateTime createdAt) : base(id)
    {
        EmployeeId = employeeId;
        ContractId = contractId;
        Time = time;
        Topic = topic;
        Notes = notes;
        CreatedAt = createdAt;
    }
}
