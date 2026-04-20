using chronos.shared.kernel.Identifiers;
using chronos.tests.shared.Factories;
using chronos.time_logs.core.Domain;
using Shouldly;

namespace chronos.time_logs.core.tests.Domain.MonthlyTimeReportTests;

public sealed class CreateTests
{
    [Fact]
    public void GivenValidParameters_WhenCreating_ThenMonthlyTimeReportIsCreated()
    {
        // Arrange
        var employeeId = EmployeeId.New();
        var period = ReportPeriodFactory.Create(month: 4, year: 2026);

        // Act
        var result = MonthlyTimeReport.Create(employeeId, period);

        // Assert
        result.Id.Value.ShouldNotBe(Ulid.Empty);
        result.EmployeeId.ShouldBe(employeeId);
        result.Period.ShouldBe(period);
        result.TimeLogs.ShouldBeEmpty();
    }
}
