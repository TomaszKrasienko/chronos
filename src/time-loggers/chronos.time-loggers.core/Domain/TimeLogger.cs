namespace chronos.time_loggers.core.Domain;

public sealed class TimeLogger
{
    public Ulid Id { get; private set; }
    public TimeSpan TimeSpan { get; private set; }
    public Ulid EmployeeId { get; private set; }
    public string Topic { get; private set; }
    public string? Notes { get; private set; }

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
        if (timeSpan is null &&
            (timeFrom is null || timeTo is null))
        {
            throw new ArgumentException("Time arguments are required.");
        }
        
        var calculatedTimeSpan = timeSpan ?? (timeTo.HasValue && timeFrom.HasValue
            ? timeTo.Value - timeFrom.Value
            : TimeSpan.Zero);

        return new TimeLogger(id, calculatedTimeSpan, employeeId, topic, notes);
    }
}