using chronos.shared.messaging;

namespace chronos.contracts.core.Events;

/// <summary>
/// Integration event published when a contract is created.
/// </summary>
/// <param name="ContractId">The contract identifier.</param>
/// <param name="CompanyName">The company name.</param>
/// <param name="AssignmentDate">The contract assignment date.</param>
/// <param name="ClosingDate">The contract closing date, or null if active.</param>
public sealed record ContractCreated(
    Ulid ContractId,
    string CompanyName,
    DateOnly AssignmentDate,
    DateOnly? ClosingDate) : IMessage;
