using chronos.contracts.core.Domain.Identifiers;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Events;

public sealed record EmployeeHoursUpdatedEvent(
    ContractId ContractId,
    ContractEmployeeId ContractEmployeeId,
    Ulid EmployeeId,
    int AllocatedHours) : IDomainEvent;
