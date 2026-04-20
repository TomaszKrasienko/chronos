using chronos.contracts.core.DTOs.Responses;
using chronos.contracts.core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

namespace chronos.contracts.core.tests.Services.CachedContractsServiceTests;

public sealed class GetAllAsyncTests
{
    [Fact]
    public async Task GivenContracts_WhenGettingAll_ThenDelegatesToService()
    {
        // Arrange
        List<ContractResponseDto> contracts =
        [
            new(
                Ulid.NewUlid().ToString(),
                "Company 1",
                new ContractPeriodResponseDto(new DateOnly(2026, 1, 1), null),
                []),
            new(
                Ulid.NewUlid().ToString(),
                "Company 2",
                new ContractPeriodResponseDto(new DateOnly(2026, 2, 1), null),
                [])
        ];
        _readContractsService
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(contracts);

        // Act
        var result = await _cachedContractsService.GetAllAsync();

        // Assert
        result.ShouldBe(contracts);
        await _readContractsService.Received(1).GetAllAsync(Arg.Any<CancellationToken>());
    }

    private readonly ILogger<CachedContractsService> _logger;
    private readonly IReadContractsService _readContractsService;
    private readonly IMemoryCache _memoryCache;
    private readonly CachedContractsService _cachedContractsService;

    public GetAllAsyncTests()
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
