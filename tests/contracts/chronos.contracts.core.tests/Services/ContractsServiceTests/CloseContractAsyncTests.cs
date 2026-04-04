using chronos.contracts.core.Communication.Sync.Http;
using chronos.contracts.core.DAL;
using chronos.contracts.core.Domain.Identifiers;
using chronos.contracts.core.Services;
using chronos.shared.kernel.Exceptions;
using chronos.shared.messaging;
using chronos.tests.shared.Factories;
using NSubstitute;
using Shouldly;

namespace chronos.contracts.core.tests.Services.ContractsServiceTests;

public sealed class CloseContractAsyncTests
{
    [Fact]
    public async Task GivenExistingContract_WhenClosingContract_ThenContractIsClosed()
    {
        // Arrange
        var contract = ContractFactory.Create();
        var closingDate = new DateOnly(2026, 12, 31);

        _contractsRepository
            .GetByIdAsync(contract.Id, Arg.Any<CancellationToken>())
            .Returns(contract);

        // Act
        await _contractsService.CloseContractAsync(contract.Id, closingDate);

        // Assert
        contract.ContractPeriod.ClosingDate.ShouldBe(closingDate);
        await _contractsRepository
            .Received(1)
            .UpdateAsync(contract, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GivenNonExistingContract_WhenClosingContract_ThenThrowsNotFoundException()
    {
        // Arrange
        var contractId = ContractId.New();
        var closingDate = new DateOnly(2026, 12, 31);

        _contractsRepository
            .GetByIdAsync(contractId, Arg.Any<CancellationToken>())
            .Returns((chronos.contracts.core.Domain.Contract?)null);

        // Act
        var act = () => _contractsService.CloseContractAsync(contractId, closingDate);

        // Assert
        await act.ShouldThrowAsync<NotFoundException>();
    }

    private readonly IContractsRepository _contractsRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IEmployeesClient _employeesClient;
    private readonly ContractsService _contractsService;

    public CloseContractAsyncTests()
    {
        _contractsRepository = Substitute.For<IContractsRepository>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _employeesClient = Substitute.For<IEmployeesClient>();
        _contractsService = new ContractsService(
            _contractsRepository,
            _messagePublisher,
            _employeesClient);
    }
}
