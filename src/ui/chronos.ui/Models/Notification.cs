namespace chronos.ui.Models;

public sealed class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public string IconClass => Type switch
    {
        "Warning" => "bi-exclamation-triangle-fill text-warning",
        "Info" => "bi-info-circle-fill text-info",
        "Success" => "bi-check-circle-fill text-success",
        "Error" => "bi-x-circle-fill text-danger",
        _ => "bi-bell-fill"
    };

    public string TimeAgo
    {
        get
        {
            var diff = DateTime.Now - Date;
            if (diff.TotalMinutes < 1) return "just now";
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} minutes ago";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} hours ago";
            return $"{(int)diff.TotalDays} days ago";
        }
    }
}
