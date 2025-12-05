namespace chronos.time_reports.core.Domain;

public sealed class MonthlyTimeReport
{
    public Ulid Id { get; }
    public Ulid EmployeeId { get; }
    public TimeSpan Summary { get; }
    public int RejectionsCount { get; }
    public int AcceptationsCount { get; }

    private MonthlyTimeReport()
    {
        
    }
    
    private MonthlyTimeReport(
        Ulid id,
        Ulid employeeId,
        TimeSpan summary,
        int rejectionsCount,
        int acceptationsCount)
    {
        Id = id;
        EmployeeId = employeeId;
        Summary = summary;
        RejectionsCount = rejectionsCount;
        AcceptationsCount = acceptationsCount;
    }

    public static MonthlyTimeReport Create(
        Ulid id,
        Ulid employeeId,
        TimeSpan summary,
        int rejectionsCount,
        int acceptationsCount)
        => new(
            id,
            employeeId,
            summary,
            rejectionsCount,
            acceptationsCount);
}