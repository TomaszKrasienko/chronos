using chronos.contracts.core.DTOs.Responses;
using chronos.contracts.core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

namespace chronos.contracts.core.tests.Services.CachedContractsServiceTests;

public sealed class CreateAsyncTests
{
    [Fact]
    public async Task GivenContract_WhenCreating_ThenAddsToCache()
    {
        // Arrange
        var contractId = Ulid.NewUlid();
        var contract = new ContractResponseDto(
            contractId.ToString(),
            "Test Company",
            new ContractPeriodResponseDto(new DateOnly(2026, 1, 1), null),
            []);

        // Act
        await _cachedContractsService.CreateAsync(contract);

        // Assert
        var cacheKey = $"contract:{contractId}";
        _memoryCache.TryGetValue<ContractResponseDto>(cacheKey, out var cachedValue).ShouldBeTrue();
        cachedValue.ShouldBe(contract);
    }

    private readonly ILogger<CachedContractsService> _logger;
    private readonly IReadContractsService _readContractsService;
    private readonly IMemoryCache _memoryCache;
    private readonly CachedContractsService _cachedContractsService;

    public CreateAsyncTests()
    {
        _logger = Substitute.For<ILogger<CachedContractsService>>();
        _readContractsService = Substitute.For<IReadContractsService>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cachedContractsService = new CachedContractsService(
            _logger,
            _readContractsService,
            _memoryCache);
    }
}
