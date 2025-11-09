namespace chronos.time_loggers.core.Domain;

public sealed class TimeLogger
{
    public Ulid Id { get; }
    public TimeSpan TimeSpan { get; }
    public Ulid EmployeeId { get; }
    public string Topic { get; }
    public string? Notes { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private TimeLogger()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    
    private TimeLogger(
        Ulid id, 
        TimeSpan timeSpan,
        Ulid employeeId,
        string topic,
        string? notes)
    {
        Id = id;
        TimeSpan = timeSpan;
        EmployeeId = employeeId;
        Topic = topic;
        Notes = notes;
    }

    public static TimeLogger Create(
        Ulid id,
        TimeSpan? timeSpan,
        TimeOnly? timeFrom,
        TimeOnly? timeTo,
        Ulid employeeId,
        string topic,
        string? notes)
    {
        if(time)
    }
}