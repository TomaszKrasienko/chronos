using chronos.contracts.core.Domain.Identifiers;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Events;

public sealed record EmployeeAssignedEvent(
    ContractId ContractId,
    ContractEmployeeId ContractEmployeeId,
    Ulid EmployeeId,
    DateOnly AssignmentFrom,
    DateOnly AssignmentTo,
    int AllocatedHours) : IDomainEvent;
