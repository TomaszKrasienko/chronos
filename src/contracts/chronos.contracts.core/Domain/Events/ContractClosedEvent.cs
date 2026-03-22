using chronos.contracts.core.Domain.Identifiers;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Events;

public sealed record ContractClosedEvent(
    ContractId ContractId,
    DateOnly ClosingDate) : IDomainEvent;
