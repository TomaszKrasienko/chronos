using chronos.contracts.core.DAL;
using chronos.contracts.core.Domain;
using chronos.contracts.core.Events.External;
using chronos.shared.kernel.Identifiers;
using chronos.tests.shared.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

namespace chronos.contracts.core.tests.Events.External.EmployeeDeletedEventHandlerTests;

public sealed class HandleTests
{
    [Fact]
    public async Task GivenContractsWithEmployee_WhenHandling_ThenEmployeeIsRemovedFromAllContracts()
    {
        // Arrange
        var employeeId = EmployeeId.New();
        var contract1 = ContractFactory.CreateWithEmployee(employeeId: employeeId);
        var contract2 = ContractFactory.CreateWithEmployee(employeeId: employeeId);
        List<Contract> contracts = [contract1, contract2];

        _contractsRepository
            .GetByEmployeeIdAsync(employeeId, Arg.Any<CancellationToken>())
            .Returns(contracts);

        SetupScopedRepository(contract1);
        SetupScopedRepository(contract2);

        var @event = new EmployeeDeleted(employeeId.Value);

        // Act
        await _handler.Handle(@event, CancellationToken.None);

        // Assert
        contract1.Employees.ShouldBeEmpty();
        contract2.Employees.ShouldBeEmpty();
    }

    [Fact]
    public async Task GivenNoContractsWithEmployee_WhenHandling_ThenNoUpdatesAreMade()
    {
        // Arrange
        var employeeId = EmployeeId.New();

        _contractsRepository
            .GetByEmployeeIdAsync(employeeId, Arg.Any<CancellationToken>())
            .Returns([]);

        var @event = new EmployeeDeleted(employeeId.Value);

        // Act
        await _handler.Handle(@event, CancellationToken.None);

        // Assert
        await _scopedRepository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Contract>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GivenContractNotFoundDuringRemoval_WhenHandling_ThenNoExceptionIsThrown()
    {
        // Arrange
        var employeeId = EmployeeId.New();
        var contract = ContractFactory.CreateWithEmployee(employeeId: employeeId);
        List<Contract> contracts = [contract];

        _contractsRepository
            .GetByEmployeeIdAsync(employeeId, Arg.Any<CancellationToken>())
            .Returns(contracts);

        _scopedRepository
            .GetByIdAsync(contract.Id, Arg.Any<CancellationToken>())
            .Returns((Contract?)null);

        var @event = new EmployeeDeleted(employeeId.Value);

        // Act
        var act = () => _handler.Handle(@event, CancellationToken.None);

        // Assert
        await act.ShouldNotThrowAsync();
        await _scopedRepository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Contract>(), Arg.Any<CancellationToken>());
    }

    private void SetupScopedRepository(Contract contract)
    {
        _scopedRepository
            .GetByIdAsync(contract.Id, Arg.Any<CancellationToken>())
            .Returns(contract);
    }

    private readonly ILogger<EmployeeDeletedEventHandler> _logger;
    private readonly IContractsRepository _contractsRepository;
    private readonly IContractsRepository _scopedRepository;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly EmployeeDeletedEventHandler _handler;

    public HandleTests()
    {
        _logger = Substitute.For<ILogger<EmployeeDeletedEventHandler>>();
        _contractsRepository = Substitute.For<IContractsRepository>();
        _scopedRepository = Substitute.For<IContractsRepository>();
        _serviceScopeFactory = Substitute.For<IServiceScopeFactory>();

        var scope = Substitute.For<IServiceScope>();
        var serviceProvider = Substitute.For<IServiceProvider>();

        _serviceScopeFactory.CreateScope().Returns(scope);
        scope.ServiceProvider.Returns(serviceProvider);
        serviceProvider.GetService(typeof(IContractsRepository)).Returns(_scopedRepository);

        _handler = new EmployeeDeletedEventHandler(
            _logger,
            _contractsRepository,
            _serviceScopeFactory);
    }
}
