namespace chronos.time_loggers.core.Exceptions;

public abstract class ChronosException(
    string code) : Exception
{
    public string Code { get; } = code;
}