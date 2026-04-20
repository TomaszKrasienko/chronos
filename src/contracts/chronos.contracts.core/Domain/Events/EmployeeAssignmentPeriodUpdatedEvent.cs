using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.contracts.core.Domain.Events;

public sealed record EmployeeAssignmentPeriodUpdatedEvent(
    ContractId ContractId,
    EmployeeId EmployeeId,
    DateOnly From,
    DateOnly? To) : IDomainEvent;
