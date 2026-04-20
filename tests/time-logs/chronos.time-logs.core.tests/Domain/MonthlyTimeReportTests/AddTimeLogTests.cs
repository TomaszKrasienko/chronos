using chronos.shared.kernel.Identifiers;
using chronos.tests.shared.Factories;
using chronos.time_logs.core.Domain;
using NSubstitute;
using Shouldly;

namespace chronos.time_logs.core.tests.Domain.MonthlyTimeReportTests;

public sealed class AddTimeLogTests
{
    [Fact]
    public void GivenValidParameters_WhenAddingTimeLog_ThenTimeLogIsWaitingForAcceptationTimeLog()
    {
        // Arrange
        var report = MonthlyTimeReportFactory.Create();
        var contractId = ContractId.New();
        var time = LoggedTimeFactory.Create();
        var topic = "Development";
        var supervisorId = EmployeeId.New();

        // Act
        report.AddTimeLog(contractId, time, topic, null, supervisorId, _timeProvider);

        // Assert
        report.TimeLogs.Count.ShouldBe(1);
        report.TimeLogs.Single().ShouldBeOfType<WaitingForAcceptationTimeLog>();
    }

    [Fact]
    public void GivenValidParameters_WhenAddingTimeLog_ThenTimeLogHasCorrectEmployeeId()
    {
        // Arrange
        var employeeId = EmployeeId.New();
        var report = MonthlyTimeReportFactory.Create(employeeId: employeeId);
        var contractId = ContractId.New();
        var time = LoggedTimeFactory.Create();

        // Act
        report.AddTimeLog(contractId, time, "Development", null, EmployeeId.New(), _timeProvider);

        // Assert
        var timeLog = report.TimeLogs.Single();
        timeLog.EmployeeId.ShouldBe(employeeId);
    }

    [Fact]
    public void GivenValidParameters_WhenAddingTimeLog_ThenTimeLogHasCorrectProperties()
    {
        // Arrange
        var report = MonthlyTimeReportFactory.Create();
        var contractId = ContractId.New();
        var time = LoggedTimeFactory.Create(TimeSpan.FromHours(8));
        var topic = "Development";
        var notes = "Some notes";
        var supervisorId = EmployeeId.New();
        var createdAt = new DateTimeOffset(2026, 4, 15, 10, 30, 0, TimeSpan.Zero);
        _timeProvider.GetUtcNow().Returns(createdAt);

        // Act
        report.AddTimeLog(contractId, time, topic, notes, supervisorId, _timeProvider);

        // Assert
        var timeLog = (WaitingForAcceptationTimeLog)report.TimeLogs.Single();
        timeLog.ContractId.ShouldBe(contractId);
        timeLog.Time.ShouldBe(time);
        timeLog.Topic.ShouldBe(topic);
        timeLog.Notes.ShouldBe(notes);
        timeLog.SupervisorId.ShouldBe(supervisorId);
        timeLog.CreatedAt.ShouldBe(createdAt.UtcDateTime);
    }

    [Fact]
    public void GivenMultipleTimeLogs_WhenAddingTimeLogs_ThenAllTimeLogsAreAdded()
    {
        // Arrange
        var report = MonthlyTimeReportFactory.Create();

        // Act
        report.AddTimeLog(ContractId.New(), LoggedTimeFactory.Create(), "Task 1", null, EmployeeId.New(), _timeProvider);
        report.AddTimeLog(ContractId.New(), LoggedTimeFactory.Create(), "Task 2", null, EmployeeId.New(), _timeProvider);

        // Assert
        report.TimeLogs.Count.ShouldBe(2);
    }

    private readonly TimeProvider _timeProvider;

    public AddTimeLogTests()
    {
        _timeProvider = Substitute.For<TimeProvider>();
        _timeProvider.GetUtcNow().Returns(DateTimeOffset.UtcNow);
    }
}
