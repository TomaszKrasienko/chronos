using chronos.contracts.core.DAL;
using chronos.contracts.core.Domain.Identifiers;
using chronos.contracts.core.Services;
using chronos.shared.kernel.Exceptions;
using chronos.tests.shared.Factories;
using NSubstitute;
using Shouldly;

namespace chronos.contracts.core.tests.Services.ContractsServiceTests;

public sealed class RemoveEmployeeAsyncTests
{
    [Fact]
    public async Task GivenContractWithEmployee_WhenRemovingEmployee_ThenEmployeeIsRemoved()
    {
        // Arrange
        var contract = ContractFactory.CreateWithEmployee();
        var contractEmployeeId = contract.Employees[0].Id;

        _contractsRepository
            .GetByIdAsync(contract.Id, Arg.Any<CancellationToken>())
            .Returns(contract);

        // Act
        await _contractsService.RemoveEmployeeAsync(contract.Id, contractEmployeeId);

        // Assert
        contract.Employees.ShouldBeEmpty();
        await _contractsRepository
            .Received(1)
            .UpdateAsync(contract, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GivenNonExistingContract_WhenRemovingEmployee_ThenThrowsNotFoundException()
    {
        // Arrange
        var contractId = ContractId.New();
        var contractEmployeeId = ContractEmployeeId.New();

        _contractsRepository
            .GetByIdAsync(contractId, Arg.Any<CancellationToken>())
            .Returns((chronos.contracts.core.Domain.Contract?)null);

        // Act
        var act = () => _contractsService.RemoveEmployeeAsync(contractId, contractEmployeeId);

        // Assert
        await act.ShouldThrowAsync<NotFoundException>();
    }

    private readonly IContractsRepository _contractsRepository;
    private readonly ContractsService _contractsService;

    public RemoveEmployeeAsyncTests()
    {
        _contractsRepository = Substitute.For<IContractsRepository>();
        _contractsService = new ContractsService(_contractsRepository);
    }
}
