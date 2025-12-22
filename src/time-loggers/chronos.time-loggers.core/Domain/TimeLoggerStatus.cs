namespace chronos.time_loggers.core.Domain;

public sealed record TimeLoggerStatus
{
    public static TimeLoggerStatus Pending => new("Pending");
    public static TimeLoggerStatus Accepted => new("Accepted");
    public static TimeLoggerStatus Rejected => new("Rejected");

    public string Value { get; }

    private TimeLoggerStatus(string value)
    {
        Value = value;
    }

    public static TimeLoggerStatus FromValue(string value)
    {
        return value switch
        {
            "Pending" => Pending,
            "Accepted" => Accepted,
            "Rejected" => Rejected,
            _ => throw new ArgumentException($"Invalid TimeLoggerStatus value: {value}", nameof(value))
        };
    }

    public override string ToString() => Value;
}