namespace chronos.ui.Models;

public sealed class NotificationMessage
{
    public string Id { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ReadAt { get; set; }

    public string IconClass => Topic switch
    {
        "TimeLogCreated" => "bi-clock-fill text-info",
        "EmployeeCreated" => "bi-person-plus-fill text-success",
        _ => "bi-bell-fill text-primary"
    };

    public string TimeAgo
    {
        get
        {
            var diff = DateTimeOffset.Now - CreatedAt;
            if (diff.TotalMinutes < 1) return "just now";
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} minutes ago";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} hours ago";
            return $"{(int)diff.TotalDays} days ago";
        }
    }
}
