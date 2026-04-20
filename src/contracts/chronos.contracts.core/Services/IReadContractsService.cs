using chronos.contracts.core.DTOs.Responses;
using chronos.shared.kernel.Identifiers;

namespace chronos.contracts.core.Services;

/// <summary>
/// Service for reading contract data.
/// </summary>
public interface IReadContractsService
{
    /// <summary>
    /// Gets a contract by its identifier.
    /// </summary>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The contract DTO if found; otherwise, null.</returns>
    Task<ContractResponseDto?> GetByIdAsync(
        ContractId contractId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all contracts.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>List of all contracts.</returns>
    Task<IReadOnlyList<ContractResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
