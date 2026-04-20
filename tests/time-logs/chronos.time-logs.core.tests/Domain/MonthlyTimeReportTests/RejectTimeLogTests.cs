using chronos.shared.kernel.Exceptions;
using chronos.shared.kernel.Identifiers;
using chronos.tests.shared.Factories;
using chronos.time_logs.core.Domain;
using NSubstitute;
using Shouldly;

namespace chronos.time_logs.core.tests.Domain.MonthlyTimeReportTests;

public sealed class RejectTimeLogTests
{
    [Fact]
    public void GivenPendingTimeLog_WhenRejecting_ThenTimeLogIsRejectedTimeLog()
    {
        // Arrange
        var report = MonthlyTimeReportFactory.CreateWithTimeLog(timeProvider: _timeProvider);
        var timeLogId = report.TimeLogs.Single().Id;
        var supervisorId = Ulid.NewUlid();
        var reason = "Hours exceed allocated limit";
        var rejectedAt = new DateTimeOffset(2026, 4, 15, 10, 30, 0, TimeSpan.Zero);
        _timeProvider.GetUtcNow().Returns(rejectedAt);

        // Act
        report.RejectTimeLog(timeLogId, supervisorId, reason, _timeProvider);

        // Assert
        report.TimeLogs.Count.ShouldBe(1);
        var rejectedLog = report.TimeLogs.Single().ShouldBeOfType<RejectedTimeLog>();
        rejectedLog.RejectedBy.ShouldBe(supervisorId);
        rejectedLog.Reason.ShouldBe(reason);
        rejectedLog.RejectedAt.ShouldBe(rejectedAt.UtcDateTime);
    }

    [Fact]
    public void GivenNonExistentTimeLog_WhenRejecting_ThenThrowsDomainExceptionWithCode_time_log_must_exist_in_pending()
    {
        // Arrange
        var report = MonthlyTimeReportFactory.Create();
        var nonExistentTimeLogId = TimeLogId.New();
        var supervisorId = Ulid.NewUlid();
        var reason = "Invalid hours";

        // Act
        var act = () => report.RejectTimeLog(nonExistentTimeLogId, supervisorId, reason, _timeProvider);

        // Assert
        var exception = act.ShouldThrow<DomainException>();
        exception.Code.ShouldBe("time_log_must_exist_in_pending");
    }

    [Fact]
    public void GivenAlreadyRejectedTimeLog_WhenRejecting_ThenThrowsDomainExceptionWithCode_time_log_must_exist_in_pending()
    {
        // Arrange
        var report = MonthlyTimeReportFactory.CreateWithTimeLog(timeProvider: _timeProvider);
        var timeLogId = report.TimeLogs.Single().Id;
        var supervisorId = Ulid.NewUlid();
        var reason = "Invalid hours";
        report.RejectTimeLog(timeLogId, supervisorId, reason, _timeProvider);

        // Act
        var act = () => report.RejectTimeLog(timeLogId, supervisorId, reason, _timeProvider);

        // Assert
        var exception = act.ShouldThrow<DomainException>();
        exception.Code.ShouldBe("time_log_must_exist_in_pending");
    }

    private readonly TimeProvider _timeProvider;

    public RejectTimeLogTests()
    {
        _timeProvider = Substitute.For<TimeProvider>();
        _timeProvider.GetUtcNow().Returns(DateTimeOffset.UtcNow);
    }
}
