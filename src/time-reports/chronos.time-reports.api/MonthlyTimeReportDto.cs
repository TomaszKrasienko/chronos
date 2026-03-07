using chronos.time_reports.core.Domain;

namespace chronos.time_reports.api;

public sealed record MonthlyTimeReportDto(
    string Id,
    string EmployeeId,
    string Summary,
    int RejectedCount,
    int AcceptedCount);

public static class MonthlyTimeReportDtoExtensions
{
    public static MonthlyTimeReportDto ToDto(this MonthlyTimeReport timeReport)
    {
        return new MonthlyTimeReportDto(
            timeReport.Id.ToString(),
            timeReport.EmployeeId.ToString(),
            timeReport.Summary.ToString(),
            timeReport.RejectedTimeLoggerIds.Count,
            timeReport.AcceptedTimeLoggerIds.Count);
    }
}
