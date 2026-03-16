namespace chronos.time_reports.core.Domain;

public sealed class MonthlyTimeReport
{
    public Ulid Id { get; private set; }
    public Ulid EmployeeId { get; private set; }
    public int Month { get; private set; }
    public int Year { get; private set; }
    public TimeSpan Summary { get; private set; }
    public List<Ulid> RejectedTimeLoggerIds { get; } = [];
    public List<Ulid> AcceptedTimeLoggerIds { get; } = [];
    
    private MonthlyTimeReport(
        Ulid id,
        Ulid employeeId,
        int month,
        int year,
        TimeSpan summary)
    {
        Id = id;
        EmployeeId = employeeId;
        Month = month;
        Year = year;       
        Summary = summary;
        RejectedTimeLoggerIds = [];
        AcceptedTimeLoggerIds = [];
    }

    public static MonthlyTimeReport Create(
        Ulid id,
        Ulid employeeId,
        int month,
        int year,
        TimeSpan summary,
        Ulid timeLoggerId,
        bool isAcceptation)
    {
        MonthlyTimeReport timeReport;
        if (isAcceptation)
        {        
            timeReport = new MonthlyTimeReport(id,
                employeeId,
                month,
                year,
                summary);
            timeReport.AcceptedTimeLoggerIds.Add(timeLoggerId);
        }
        else
        {
            timeReport = new MonthlyTimeReport(id,
                employeeId,
                month,
                year,
                TimeSpan.Zero);
            timeReport.RejectedTimeLoggerIds.Add(timeLoggerId);
        }
        
        return timeReport;
    }

    public void Update(Ulid timeLoggerId, TimeSpan timeSpan, bool isAcceptation)
    {
        var targetList = isAcceptation ? AcceptedTimeLoggerIds : RejectedTimeLoggerIds;

        if (targetList.Contains(timeLoggerId))
        {
            return;
        }

        if (isAcceptation)
        {
            Summary += timeSpan;
        }
        
        targetList.Add(timeLoggerId);
    }
}