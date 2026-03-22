using chronos.contracts.core.Domain.Identifiers;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Events;

public sealed record ContractCreatedEvent(
    ContractId ContractId,
    string CompanyName,
    DateOnly AssignmentDate,
    DateOnly? ClosingDate) : IDomainEvent;
