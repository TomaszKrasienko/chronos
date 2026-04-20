using chronos.shared.kernel.Exceptions;
using chronos.shared.kernel.Identifiers;
using chronos.tests.shared.Factories;
using chronos.time_logs.core.Domain;
using NSubstitute;
using Shouldly;

namespace chronos.time_logs.core.tests.Domain.MonthlyTimeReportTests;

public sealed class AcceptTimeLogTests
{
    [Fact]
    public void GivenPendingTimeLog_WhenAccepting_ThenTimeLogIsAcceptedTimeLog()
    {
        // Arrange
        var report = MonthlyTimeReportFactory.CreateWithTimeLog(timeProvider: _timeProvider);
        var timeLogId = report.TimeLogs.Single().Id;
        var supervisorId = Ulid.NewUlid();
        var acceptedAt = new DateTimeOffset(2026, 4, 15, 10, 30, 0, TimeSpan.Zero);
        _timeProvider.GetUtcNow().Returns(acceptedAt);

        // Act
        report.AcceptTimeLog(timeLogId, supervisorId, _timeProvider);

        // Assert
        report.TimeLogs.Count.ShouldBe(1);
        var acceptedLog = report.TimeLogs.Single().ShouldBeOfType<AcceptedTimeLog>();
        acceptedLog.AcceptedBy.ShouldBe(supervisorId);
        acceptedLog.AcceptedAt.ShouldBe(acceptedAt.UtcDateTime);
    }

    [Fact]
    public void GivenNonExistentTimeLog_WhenAccepting_ThenThrowsDomainExceptionWithCode_time_log_must_exist_in_pending()
    {
        // Arrange
        var report = MonthlyTimeReportFactory.Create();
        var nonExistentTimeLogId = TimeLogId.New();
        var supervisorId = Ulid.NewUlid();

        // Act
        var act = () => report.AcceptTimeLog(nonExistentTimeLogId, supervisorId, _timeProvider);

        // Assert
        var exception = act.ShouldThrow<DomainException>();
        exception.Code.ShouldBe("time_log_must_exist_in_pending");
    }

    [Fact]
    public void GivenAlreadyAcceptedTimeLog_WhenAccepting_ThenThrowsDomainExceptionWithCode_time_log_must_exist_in_pending()
    {
        // Arrange
        var report = MonthlyTimeReportFactory.CreateWithTimeLog(timeProvider: _timeProvider);
        var timeLogId = report.TimeLogs.Single().Id;
        var supervisorId = Ulid.NewUlid();
        report.AcceptTimeLog(timeLogId, supervisorId, _timeProvider);

        // Act
        var act = () => report.AcceptTimeLog(timeLogId, supervisorId, _timeProvider);

        // Assert
        var exception = act.ShouldThrow<DomainException>();
        exception.Code.ShouldBe("time_log_must_exist_in_pending");
    }

    private readonly TimeProvider _timeProvider;

    public AcceptTimeLogTests()
    {
        _timeProvider = Substitute.For<TimeProvider>();
        _timeProvider.GetUtcNow().Returns(DateTimeOffset.UtcNow);
    }
}
