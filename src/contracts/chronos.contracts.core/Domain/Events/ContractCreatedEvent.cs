using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.contracts.core.Domain.Events;

public sealed record ContractCreatedEvent(
    ContractId ContractId,
    string CompanyName,
    DateOnly AssignmentDate,
    DateOnly? ClosingDate) : IDomainEvent;
