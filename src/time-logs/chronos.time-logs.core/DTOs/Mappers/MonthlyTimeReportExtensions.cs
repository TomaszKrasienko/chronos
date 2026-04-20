using chronos.time_logs.core.DTOs.Responses;

// ReSharper disable once CheckNamespace
namespace chronos.time_logs.core.Domain;

public static class MonthlyTimeReportExtensions
{
    private const string UnknwonTimeLogStatus = "Unknown";
    
    public static MonthlyTimeReportResponseDto ToDto(this MonthlyTimeReport report)
        => new(
            report.Id.Value.ToString(),
            report.EmployeeId.Value.ToString(),
            report.Period.Month,
            report.Period.Year,
            report.TimeLogs.Select(ToTimeLogDto).ToList());

    private static TimeLogResponseDto ToTimeLogDto(TimeLog timeLog)
        => new(
            timeLog.Id.Value.ToString(),
            timeLog.ContractId.Value.ToString(),
            timeLog.Time.Value,
            timeLog.Topic,
            timeLog.Notes,
            GetStatus(timeLog),
            new DateTimeOffset(timeLog.CreatedAt, TimeSpan.Zero));

    private static string GetStatus(TimeLog timeLog) => timeLog switch
    {
        WaitingForAcceptationTimeLog => WaitingForAcceptationTimeLog.StatusName,
        AcceptedTimeLog => AcceptedTimeLog.StatusName,
        RejectedTimeLog => RejectedTimeLog.StatusName,
        _ => UnknwonTimeLogStatus
    };
}
