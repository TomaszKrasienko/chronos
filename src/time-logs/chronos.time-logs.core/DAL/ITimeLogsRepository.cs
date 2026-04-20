using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain;

namespace chronos.time_logs.core.DAL;

/// <summary>
/// Repository for managing monthly time reports.
/// </summary>
public interface ITimeLogsRepository
{
    /// <summary>
    /// Gets a monthly time report by identifier.
    /// </summary>
    /// <param name="id">The report identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<MonthlyTimeReport?> GetByIdAsync(
        MonthlyTimeReportId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a monthly time report containing the specified time log.
    /// </summary>
    /// <param name="timeLogId">The time log identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<MonthlyTimeReport?> GetByTimeLogIdAsync(
        TimeLogId timeLogId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a monthly time report by employee, month and year.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="month">The month (1-12).</param>
    /// <param name="year">The year.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<MonthlyTimeReport?> GetByEmployeeAndPeriodAsync(
        EmployeeId employeeId,
        int month,
        int year,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new monthly time report.
    /// </summary>
    /// <param name="report">The report to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AddAsync(
        MonthlyTimeReport report,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing monthly time report.
    /// </summary>
    /// <param name="report">The report to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UpdateAsync(
        MonthlyTimeReport report,
        CancellationToken cancellationToken = default);
}
