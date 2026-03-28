using chronos.contracts.core.DTOs.Responses;

namespace chronos.contracts.core.Services;

/// <summary>
/// Service for managing contracts cache operations.
/// </summary>
public interface IContractsCache
{
    /// <summary>
    /// Adds a contract to the cache.
    /// </summary>
    /// <param name="contract">The contract response DTO to cache.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task CreateAsync(
        ContractResponseDto contract,
        CancellationToken cancellationToken = default);
}
