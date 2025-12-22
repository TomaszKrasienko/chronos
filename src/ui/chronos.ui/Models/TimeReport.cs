namespace chronos.ui.Models;

public sealed class TimeReport
{
    public string EmployeeId { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public decimal TotalHours { get; set; }
    public int AcceptedEntries { get; set; }
    public int PendingEntries { get; set; }
    public int RejectedEntries { get; set; }
}
