using System.Text.Json;
using chronos.time_reports.core.Configuration;
using chronos.time_reports.core.DAL;
using chronos.time_reports.core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace chronos.time_reports.core.Services;

public interface ITimeReportsService
{
    Task UpsertTimeReportAsync(
        Ulid timeLoggerId,
        Ulid employeeId,
        TimeSpan timeSpan,
        bool isAcceptation);

    Task<MonthlyTimeReport?> GetByEmployeeIdAsync(
        Ulid employeeId,
        CancellationToken cancellationToken = default);

    Task SaveToFileAsync(Ulid employeeId, CancellationToken cancellationToken = default);
}

internal sealed class TimeReportsService(
    TimeReportsDbContext dbContext,
    TimeProvider timeProvider,
    IOptions<SavingOptions> savingOptions) : ITimeReportsService
{
    public async Task UpsertTimeReportAsync(Ulid timeLoggerId, Ulid employeeId, TimeSpan timeSpan, bool isAcceptation)
    {
        var now = timeProvider.GetUtcNow();
        
        var existingReport = await dbContext
            .MonthlyTimeReports
            .FirstOrDefaultAsync(x 
                => x.EmployeeId == employeeId
                && x.Year == now.Year
                && x.Month == now.Month);

        if (existingReport is not null)
        {
            existingReport.Update(timeLoggerId, timeSpan, isAcceptation);
        }
        else
        {
            var newReport = MonthlyTimeReport.Create(
                Ulid.NewUlid(),
                employeeId,
                now.Month,
                now.Year,
                timeSpan,
                timeLoggerId,
                isAcceptation);

            await dbContext.MonthlyTimeReports.AddAsync(newReport);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task<MonthlyTimeReport?> GetByEmployeeIdAsync(
        Ulid employeeId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext
            .MonthlyTimeReports
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId, cancellationToken);
    }

    public async Task SaveToFileAsync(Ulid employeeId, CancellationToken cancellationToken = default)
    {
        var report = await GetByEmployeeIdAsync(employeeId, cancellationToken);

        if (report is null)
        {
            return;
        }

        var fileName = $"{employeeId}_{report.Month}_{report.Year}.json";
        var filePath = Path.Combine(savingOptions.Value.Path, fileName);

        var json = JsonSerializer.Serialize(report, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        Directory.CreateDirectory(savingOptions.Value.Path);
        await File.WriteAllTextAsync(filePath, json, cancellationToken);
    }
}