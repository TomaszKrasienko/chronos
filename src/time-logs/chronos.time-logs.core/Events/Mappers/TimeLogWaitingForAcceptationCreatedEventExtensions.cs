using chronos.time_logs.core.Domain.Events;

namespace chronos.time_logs.core.Events.Mappers;

/// <summary>
/// Extension methods for mapping <see cref="TimeLogWaitingForAcceptationCreatedEvent"/> to integration event.
/// </summary>
public static class TimeLogWaitingForAcceptationCreatedEventExtensions
{
    /// <summary>
    /// Converts the domain event to an integration event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="totalAcceptedHoursInMonth">The total accepted hours for this contract in the current month.</param>
    public static TimeLogWaitingForAcceptationCreated ToIntegrationEvent(
        this TimeLogWaitingForAcceptationCreatedEvent domainEvent,
        TimeSpan totalAcceptedHoursInMonth)
        => new(
            domainEvent.TimeLogId.Value,
            domainEvent.ContractId.Value,
            domainEvent.EmployeeId.Value,
            domainEvent.Time,
            domainEvent.Topic,
            domainEvent.Notes,
            domainEvent.SupervisorId.Value,
            totalAcceptedHoursInMonth);
}
