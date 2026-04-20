using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.contracts.core.Domain.Events;

public sealed record ContractClosedEvent(
    ContractId ContractId,
    DateOnly ClosingDate) : IDomainEvent;
