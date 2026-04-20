using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.contracts.core.Domain.Events;

public sealed record EmployeeRemovedEvent(
    ContractId ContractId,
    EmployeeId EmployeeId) : IDomainEvent;
