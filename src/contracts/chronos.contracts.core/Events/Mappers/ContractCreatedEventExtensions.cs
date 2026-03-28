using chronos.contracts.core.Events;

namespace chronos.contracts.core.Domain.Events;

/// <summary>
/// Extension methods for mapping <see cref="ContractCreatedEvent"/> to integration events.
/// </summary>
public static class ContractCreatedEventExtensions
{
    /// <summary>
    /// Maps a <see cref="ContractCreatedEvent"/> to <see cref="ContractCreated"/> integration event.
    /// </summary>
    /// <param name="domainEvent">The domain event to map.</param>
    /// <returns>The integration event.</returns>
    public static ContractCreated ToIntegrationEvent(this ContractCreatedEvent domainEvent)
        => new(
            domainEvent.ContractId.Value,
            domainEvent.CompanyName,
            domainEvent.AssignmentDate,
            domainEvent.ClosingDate);
}
