using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.contracts.core.Domain.Events;

public sealed record EmployeeHoursUpdatedEvent(
    ContractId ContractId,
    EmployeeId EmployeeId,
    int AllocatedHours) : IDomainEvent;
