namespace chronos.ui.Models;

public sealed class TimeReport
{
    public string Id { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public int RejectedCount { get; set; }
    public int AcceptedCount { get; set; }

    // Computed properties for UI
    public string EmployeeName { get; set; } = string.Empty;
    public string Period { get; set; } = "Current Month";

    public decimal TotalHours
    {
        get
        {
            if (TimeSpan.TryParse(Summary, out var timeSpan))
            {
                return (decimal)timeSpan.TotalHours;
            }
            return 0;
        }
    }

    public int AcceptedEntries => AcceptedCount;
    public int RejectedEntries => RejectedCount;
    public int PendingEntries => 0; // Not available from API
}
