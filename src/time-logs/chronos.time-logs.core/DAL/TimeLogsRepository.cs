using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain;
using Microsoft.EntityFrameworkCore;

namespace chronos.time_logs.core.DAL;

internal sealed class TimeLogsRepository(TimeLogsDbContext dbContext) : ITimeLogsRepository
{
    /// <inheritdoc />
    public async Task<MonthlyTimeReport?> GetByIdAsync(
        MonthlyTimeReportId id,
        CancellationToken cancellationToken = default)
        => await dbContext
            .MonthlyTimeReports
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<MonthlyTimeReport?> GetByTimeLogIdAsync(
        TimeLogId timeLogId,
        CancellationToken cancellationToken = default)
        => await dbContext
            .MonthlyTimeReports
            .SingleOrDefaultAsync(
                x => x.PendingTimeLogs.Any(t => t.Id == timeLogId)
                     || x.AcceptedTimeLogs.Any(t => t.Id == timeLogId)
                     || x.RejectedTimeLogs.Any(t => t.Id == timeLogId),
                cancellationToken);

    /// <inheritdoc />
    public async Task<MonthlyTimeReport?> GetByEmployeeAndPeriodAsync(
        EmployeeId employeeId,
        int month,
        int year,
        CancellationToken cancellationToken = default)
        => await dbContext
            .MonthlyTimeReports
            .SingleOrDefaultAsync(
                x => x.EmployeeId == employeeId && x.Period.Month == month && x.Period.Year == year,
                cancellationToken);

    /// <inheritdoc />
    public async Task AddAsync(
        MonthlyTimeReport report,
        CancellationToken cancellationToken = default)
    {
        await dbContext.MonthlyTimeReports.AddAsync(report, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(
        MonthlyTimeReport report,
        CancellationToken cancellationToken = default)
    {
        dbContext.MonthlyTimeReports.Update(report);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
