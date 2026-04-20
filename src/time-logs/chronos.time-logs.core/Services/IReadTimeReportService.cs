using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain;

namespace chronos.time_logs.core.Services;

/// <summary>
/// Service for reading time reports.
/// </summary>
public interface IReadTimeReportService
{
    /// <summary>
    /// Gets the report for an employee by month and year.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="month">The month (1-12).</param>
    /// <param name="year">The year.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The monthly time report or null if not found.</returns>
    Task<MonthlyTimeReport?> GetByEmployeeAndPeriodAsync(
        EmployeeId employeeId,
        int month,
        int year,
        CancellationToken cancellationToken = default);
}
