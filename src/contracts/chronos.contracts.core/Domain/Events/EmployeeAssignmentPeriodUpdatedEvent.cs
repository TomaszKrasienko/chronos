using chronos.contracts.core.Domain.Identifiers;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Events;

public sealed record EmployeeAssignmentPeriodUpdatedEvent(
    ContractId ContractId,
    ContractEmployeeId ContractEmployeeId,
    Ulid EmployeeId,
    DateOnly From,
    DateOnly? To) : IDomainEvent;
