using System.ComponentModel.DataAnnotations;

namespace chronos.ui.Models;

public sealed class Employee
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    public string? SupervisorId { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
