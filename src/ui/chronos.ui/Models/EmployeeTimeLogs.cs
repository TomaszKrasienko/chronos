namespace chronos.ui.Models;

public sealed record EmployeeTimeLogs(
    string EmployeeId,
    string FirstName,
    string LastName,
    List<TimeLog> TimeLogs);
