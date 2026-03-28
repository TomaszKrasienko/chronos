using chronos.contracts.core.Domain.Identifiers;
using chronos.contracts.core.DTOs.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace chronos.contracts.core.Services;

/// <summary>
/// Cached decorator for <see cref="IReadContractsService"/>.
/// </summary>
internal sealed class CachedContractsService(
    ILogger<CachedContractsService> logger,
    IReadContractsService readContractsService,
    IMemoryCache memoryCache) : IReadContractsService, IContractsCache
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    /// <inheritdoc />
    public async Task<ContractResponseDto?> GetByIdAsync(
        ContractId contractId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting contract with ID {ContractId} from cache", contractId.Value);
        var cacheKey = GetCacheKey(contractId.Value);

        if (memoryCache.TryGetValue<ContractResponseDto>(cacheKey, out var cachedContract))
        {
            return cachedContract;
        }

        var contract = await readContractsService.GetByIdAsync(contractId, cancellationToken);

        if (contract is not null)
        {
            memoryCache.Set(cacheKey, contract, CacheDuration);
        }

        return contract;
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ContractResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
        => readContractsService.GetAllAsync(cancellationToken);

    /// <inheritdoc />
    public Task CreateAsync(
        ContractResponseDto contract,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = GetCacheKey(Ulid.Parse(contract.Id));
        memoryCache.Set(cacheKey, contract, CacheDuration);

        return Task.CompletedTask;
    }

    private static string GetCacheKey(Ulid contractId) => $"contract:{contractId}";
}
