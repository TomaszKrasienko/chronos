using chronos.shared.kernel.Exceptions;
using chronos.shared.kernel.Identifiers;
using chronos.shared.messaging;
using chronos.time_logs.core.Communication.Sync;
using chronos.time_logs.core.DAL;
using chronos.time_logs.core.Domain;
using chronos.time_logs.core.Domain.ValueObjects;
using chronos.time_logs.core.Events;
using chronos.time_logs.core.Services;
using NSubstitute;
using Shouldly;

namespace chronos.time_logs.core.tests.Services.TimeReportServiceTests;

public sealed class AddTimeLogAsyncTests
{
    [Fact]
    public async Task GivenEmployeeNotAssignedToContract_WhenAddingTimeLog_ThenThrowsNotFoundException()
    {
        // Arrange
        var employeeId = EmployeeId.New();
        var contractId = ContractId.New();
        var time = TimeSpan.FromHours(2);
        var topic = "Test topic";

        _contractsClient
            .IsEmployeeAssignedToContractAsync(contractId.Value, employeeId.Value, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var act = () => _timeReportService.AddTimeLogAsync(
            employeeId,
            contractId,
            time,
            topic,
            null);

        // Assert
        var exception = await act.ShouldThrowAsync<NotFoundException>();
        exception.Code.ShouldBe("employee_assignment_not_found");
    }

    [Fact]
    public async Task GivenNoExistingReport_WhenAddingTimeLog_ThenCreatesNewReportAndAddsTimeLog()
    {
        // Arrange
        var employeeId = EmployeeId.New();
        var contractId = ContractId.New();
        var time = TimeSpan.FromHours(2);
        var topic = "Test topic";
        var notes = "Test notes";
        var supervisorId = Ulid.NewUlid();

        _contractsClient
            .IsEmployeeAssignedToContractAsync(contractId.Value, employeeId.Value, Arg.Any<CancellationToken>())
            .Returns(true);

        _employeesClient
            .GetByIdAsync(employeeId.Value, Arg.Any<CancellationToken>())
            .Returns(new EmployeeDto(employeeId.Value, "John", "Doe", supervisorId));

        _timeLogsRepository
            .GetByEmployeeAndPeriodAsync(employeeId, _testDate.Month, _testDate.Year, Arg.Any<CancellationToken>())
            .Returns((MonthlyTimeReport?)null);

        // Act
        var result = await _timeReportService.AddTimeLogAsync(
            employeeId,
            contractId,
            time,
            topic,
            notes);

        // Assert
        result.Value.ShouldNotBe(Ulid.Empty);
        await _timeLogsRepository
            .Received(1)
            .AddAsync(
                Arg.Is<MonthlyTimeReport>(r =>
                    r.EmployeeId == employeeId &&
                    r.Period.Month == _testDate.Month &&
                    r.Period.Year == _testDate.Year &&
                    r.TimeLogs.Count == 1),
                Arg.Any<CancellationToken>());
        await _timeLogsRepository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<MonthlyTimeReport>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GivenExistingReport_WhenAddingTimeLog_ThenUpdatesReport()
    {
        // Arrange
        var employeeId = EmployeeId.New();
        var contractId = ContractId.New();
        var time = TimeSpan.FromHours(2);
        var topic = "Test topic";
        var supervisorId = Ulid.NewUlid();

        var existingReport = MonthlyTimeReport.Create(
            employeeId,
            ReportPeriod.Create(_testDate.Month, _testDate.Year));

        _contractsClient
            .IsEmployeeAssignedToContractAsync(contractId.Value, employeeId.Value, Arg.Any<CancellationToken>())
            .Returns(true);

        _employeesClient
            .GetByIdAsync(employeeId.Value, Arg.Any<CancellationToken>())
            .Returns(new EmployeeDto(employeeId.Value, "John", "Doe", supervisorId));

        _timeLogsRepository
            .GetByEmployeeAndPeriodAsync(employeeId, _testDate.Month, _testDate.Year, Arg.Any<CancellationToken>())
            .Returns(existingReport);

        // Act
        var result = await _timeReportService.AddTimeLogAsync(
            employeeId,
            contractId,
            time,
            topic,
            null);

        // Assert
        result.Value.ShouldNotBe(Ulid.Empty);
        await _timeLogsRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<MonthlyTimeReport>(), Arg.Any<CancellationToken>());
        await _timeLogsRepository
            .Received(1)
            .UpdateAsync(
                Arg.Is<MonthlyTimeReport>(r =>
                    r.Id == existingReport.Id &&
                    r.TimeLogs.Count == 1),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GivenValidParameters_WhenAddingTimeLog_ThenDispatchesIntegrationEvent()
    {
        // Arrange
        var employeeId = EmployeeId.New();
        var contractId = ContractId.New();
        var time = TimeSpan.FromHours(2);
        var topic = "Test topic";
        var notes = "Test notes";
        var supervisorId = Ulid.NewUlid();

        _contractsClient
            .IsEmployeeAssignedToContractAsync(contractId.Value, employeeId.Value, Arg.Any<CancellationToken>())
            .Returns(true);

        _employeesClient
            .GetByIdAsync(employeeId.Value, Arg.Any<CancellationToken>())
            .Returns(new EmployeeDto(employeeId.Value, "John", "Doe", supervisorId));

        _timeLogsRepository
            .GetByEmployeeAndPeriodAsync(employeeId, _testDate.Month, _testDate.Year, Arg.Any<CancellationToken>())
            .Returns((MonthlyTimeReport?)null);

        // Act
        await _timeReportService.AddTimeLogAsync(
            employeeId,
            contractId,
            time,
            topic,
            notes);

        // Assert
        await _messageDispatcher
            .Received(1)
            .Send(
                Arg.Is<TimeLogWaitingForAcceptationCreated>(e =>
                    e.ContractId == contractId.Value &&
                    e.EmployeeId == employeeId.Value &&
                    e.Time == time &&
                    e.Topic == topic &&
                    e.Notes == notes &&
                    e.SupervisorId == supervisorId),
                Arg.Any<CancellationToken>());
    }

    private readonly DateTime _testDate = new(2026, 4, 15, 10, 0, 0, DateTimeKind.Utc);
    private readonly ITimeLogsRepository _timeLogsRepository;
    private readonly IContractsClient _contractsClient;
    private readonly IEmployeesClient _employeesClient;
    private readonly IMessageDispatcher _messageDispatcher;
    private readonly TimeProvider _timeProvider;
    private readonly TimeReportService _timeReportService;

    public AddTimeLogAsyncTests()
    {
        _timeLogsRepository = Substitute.For<ITimeLogsRepository>();
        _contractsClient = Substitute.For<IContractsClient>();
        _employeesClient = Substitute.For<IEmployeesClient>();
        _messageDispatcher = Substitute.For<IMessageDispatcher>();
        _timeProvider = Substitute.For<TimeProvider>();
        _timeProvider.GetUtcNow().Returns(new DateTimeOffset(_testDate));

        _timeReportService = new TimeReportService(
            _timeLogsRepository,
            _contractsClient,
            _employeesClient,
            _messageDispatcher,
            _timeProvider);
    }
}
