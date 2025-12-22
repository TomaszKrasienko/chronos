using System.ComponentModel.DataAnnotations;

namespace chronos.ui.Models;

public sealed class CreateTimeLogRequest
{
    public TimeSpan? TimeSpan { get; set; }

    public TimeOnly? TimeFrom { get; set; }

    public TimeOnly? TimeTo { get; set; }

    [Required(ErrorMessage = "Topic is required")]
    public string Topic { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
