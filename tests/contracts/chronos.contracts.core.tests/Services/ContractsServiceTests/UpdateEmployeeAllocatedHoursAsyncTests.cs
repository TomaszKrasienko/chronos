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

public sealed class UpdateEmployeeAllocatedHoursAsyncTests
{
    [Fact]
    public async Task GivenContractWithEmployee_WhenUpdatingAllocatedHours_ThenHoursAreUpdated()
    {
        // Arrange
        var contract = ContractFactory.CreateWithEmployee(allocatedHours: 160);
        var contractEmployeeId = contract.Employees[0].Id;
        var newAllocatedHours = 200;

        _contractsRepository.GetByIdAsync(contract.Id, Arg.Any<CancellationToken>())
            .Returns(contract);

        // Act
        await _contractsService.UpdateEmployeeAllocatedHoursAsync(contract.Id, contractEmployeeId, newAllocatedHours);

        // Assert
        contract.Employees[0].AllocatedHours.ShouldBe(newAllocatedHours);
        await _contractsRepository.Received(1).UpdateAsync(contract, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GivenNonExistingContract_WhenUpdatingAllocatedHours_ThenThrowsNotFoundException()
    {
        // Arrange
        var contractId = ContractId.New();
        var contractEmployeeId = ContractEmployeeId.New();
        var newAllocatedHours = 200;

        _contractsRepository.GetByIdAsync(contractId, Arg.Any<CancellationToken>())
            .Returns((chronos.contracts.core.Domain.Contract?)null);

        // Act
        var act = () => _contractsService.UpdateEmployeeAllocatedHoursAsync(contractId, contractEmployeeId, newAllocatedHours);

        // Assert
        await act.ShouldThrowAsync<NotFoundException>();
    }

    private readonly IContractsRepository _contractsRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IEmployeesClient _employeesClient;
    private readonly ContractsService _contractsService;

    public UpdateEmployeeAllocatedHoursAsyncTests()
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
