using chronos.contracts.core.DAL;
using chronos.contracts.core.Domain.Identifiers;
using chronos.contracts.core.Services;
using chronos.shared.kernel.Exceptions;
using chronos.shared.messaging;
using chronos.tests.shared.Factories;
using NSubstitute;
using Shouldly;

namespace chronos.contracts.core.tests.Services.ContractsServiceTests;

public sealed class AssignEmployeeAsyncTests
{
    [Fact]
    public async Task GivenExistingContract_WhenAssigningEmployee_ThenEmployeeIsAssigned()
    {
        // Arrange
        var contract = ContractFactory.Create();
        var employeeId = Ulid.NewUlid();
        var from = new DateOnly(2026, 4, 1);
        var to = new DateOnly(2026, 12, 31);
        var allocatedHours = 160;

        _contractsRepository
            .GetByIdAsync(contract.Id, Arg.Any<CancellationToken>())
            .Returns(contract);

        // Act
        await _contractsService.AssignEmployeeAsync(contract.Id, employeeId, from, to, allocatedHours);

        // Assert
        contract.Employees.Count.ShouldBe(1);
        contract.Employees[0].EmployeeId.ShouldBe(employeeId);
        contract.Employees[0].AllocatedHours.ShouldBe(allocatedHours);
        await _contractsRepository.Received(1).UpdateAsync(contract, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GivenNonExistingContract_WhenAssigningEmployee_ThenThrowsNotFoundException()
    {
        // Arrange
        var contractId = ContractId.New();
        var employeeId = Ulid.NewUlid();
        var from = new DateOnly(2026, 4, 1);
        var to = new DateOnly(2026, 12, 31);
        var allocatedHours = 160;

        _contractsRepository.GetByIdAsync(contractId, Arg.Any<CancellationToken>())
            .Returns((chronos.contracts.core.Domain.Contract?)null);

        // Act
        var act = () => _contractsService.AssignEmployeeAsync(contractId, employeeId, from, to, allocatedHours);

        // Assert
        await act.ShouldThrowAsync<NotFoundException>();
    }

    private readonly IContractsRepository _contractsRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ContractsService _contractsService;

    public AssignEmployeeAsyncTests()
    {
        _contractsRepository = Substitute.For<IContractsRepository>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _contractsService = new ContractsService(_contractsRepository, _messagePublisher);
    }
}
