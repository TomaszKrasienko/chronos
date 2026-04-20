using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.contracts.core.Domain.Events;

public sealed record EmployeeAssignedEvent(
    ContractId ContractId,
    EmployeeId EmployeeId,
    DateOnly AssignmentFrom,
    DateOnly? AssignmentTo,
    int AllocatedHours) : IDomainEvent;
