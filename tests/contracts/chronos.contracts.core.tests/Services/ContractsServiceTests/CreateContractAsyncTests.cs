using chronos.contracts.core.DAL;
using chronos.contracts.core.Domain;
using chronos.contracts.core.Services;
using chronos.shared.kernel.Exceptions;
using chronos.shared.messaging;
using NSubstitute;
using Shouldly;

namespace chronos.contracts.core.tests.Services.ContractsServiceTests;

public sealed class CreateContractAsyncTests
{
    [Fact]
    public async Task GivenValidParameters_WhenCreatingContract_ThenContractIsCreatedAndIdReturned()
    {
        // Arrange
        var companyName = "Test Company";
        var assignmentDate = new DateOnly(2026, 4, 1);
        _contractsRepository
            .ExistsByCompanyNameAsync(companyName, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _contractsService.CreateContractAsync(companyName, assignmentDate);

        // Assert
        result.Value.ShouldNotBe(Ulid.Empty);
        await _contractsRepository
            .Received(1)
            .AddAsync(
            Arg.Is<Contract>(c =>
                c.CompanyDetails.Name == companyName &&
                c.ContractPeriod.AssignmentDate == assignmentDate),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GivenExistingCompanyName_WhenCreatingContract_ThenThrowsNotUniqueException()
    {
        // Arrange
        var companyName = "Existing Company";
        var assignmentDate = new DateOnly(2026, 4, 1);
        _contractsRepository
            .ExistsByCompanyNameAsync(companyName, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var act = () => _contractsService.CreateContractAsync(companyName, assignmentDate);

        // Assert
        var exception = await act.ShouldThrowAsync<NotUniqueException>();
        exception.EntityName.ShouldBe(nameof(Contract));
    }

    private readonly IContractsRepository _contractsRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ContractsService _contractsService;

    public CreateContractAsyncTests()
    {
        _contractsRepository = Substitute.For<IContractsRepository>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _contractsService = new ContractsService(_contractsRepository, _messagePublisher);
    }
}
