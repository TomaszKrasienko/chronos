namespace chronos.ui.Models;

public sealed class TimeLog
{
    public string Id { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string? EmployeeName { get; set; }
    public TimeSpan TimeSpan { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string Status { get; set; } = string.Empty;

    public string FormattedHours => $"{TimeSpan.TotalHours:F2}h";
    public string StatusBadgeClass => Status switch
    {
        "Pending" => "bg-warning",
        "Accepted" => "bg-success",
        "Rejected" => "bg-danger",
        _ => "bg-secondary"
    };
}
