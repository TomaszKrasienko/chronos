namespace chronos.time_logs.core.DTOs.Responses;

public sealed record MonthlyTimeReportResponseDto(
    string Id,
    string EmployeeId,
    int Month,
    int Year,
    IReadOnlyCollection<TimeLogResponseDto> TimeLogs);
