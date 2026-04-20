using chronos.shared.kernel.Identifiers;
using chronos.contracts.core.DTOs.Responses;
using chronos.contracts.core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

namespace chronos.contracts.core.tests.Services.CachedContractsServiceTests;

public sealed class GetByIdAsyncTests
{
    [Fact]
    public async Task GivenContractInCache_WhenGettingById_ThenReturnsCachedContract()
    {
        // Arrange
        var contractId = ContractId.New();
        var cachedContract = CreateContractResponseDto(contractId);
        var cacheKey = $"contract:{contractId.Value}";
        _memoryCache.Set(cacheKey, cachedContract);

        // Act
        var result = await _cachedContractsService.GetByIdAsync(contractId);

        // Assert
        result.ShouldBe(cachedContract);
        await _readContractsService
            .DidNotReceive()
            .GetByIdAsync(Arg.Any<ContractId>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GivenContractNotInCache_WhenGettingById_ThenFetchesFromServiceAndCaches()
    {
        // Arrange
        var contractId = ContractId.New();
        var contract = CreateContractResponseDto(contractId);
        _readContractsService
            .GetByIdAsync(contractId, Arg.Any<CancellationToken>())
            .Returns(contract);

        // Act
        var result = await _cachedContractsService.GetByIdAsync(contractId);

        // Assert
        result.ShouldBe(contract);
        var cacheKey = $"contract:{contractId.Value}";
        _memoryCache.TryGetValue<ContractResponseDto>(cacheKey, out var cachedValue).ShouldBeTrue();
        cachedValue.ShouldBe(contract);
    }

    [Fact]
    public async Task GivenContractNotFound_WhenGettingById_ThenReturnsNullAndDoesNotCache()
    {
        // Arrange
        var contractId = ContractId.New();
        _readContractsService
            .GetByIdAsync(contractId, Arg.Any<CancellationToken>())
            .Returns((ContractResponseDto?)null);

        // Act
        var result = await _cachedContractsService.GetByIdAsync(contractId);

        // Assert
        result.ShouldBeNull();
        var cacheKey = $"contract:{contractId.Value}";
        _memoryCache.TryGetValue<ContractResponseDto>(cacheKey, out _).ShouldBeFalse();
    }

    private static ContractResponseDto CreateContractResponseDto(ContractId contractId)
        => new(
            contractId.Value.ToString(),
            "Test Company",
            new ContractPeriodResponseDto(new DateOnly(2026, 1, 1), null),
            []);

    private readonly ILogger<CachedContractsService> _logger;
    private readonly IReadContractsService _readContractsService;
    private readonly IMemoryCache _memoryCache;
    private readonly CachedContractsService _cachedContractsService;

    public GetByIdAsyncTests()
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
