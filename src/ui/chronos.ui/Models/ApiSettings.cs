namespace chronos.ui.Models;

public class ApiSettings
{
    public string Environment { get; set; } = "Kubernetes";
    public ApiEndpoints Local { get; set; } = new();
    public ApiEndpoints Docker { get; set; } = new();
    public ApiEndpoints Kubernetes { get; set; } = new();

    public ApiEndpoints GetCurrentEndpoints()
    {
        return Environment.ToLower() switch
        {
            _ => Kubernetes
        };
    }
}

public class ApiEndpoints
{
    public string EmployeesApi { get; set; } = string.Empty;
    public string TimeLoggersApi { get; set; } = string.Empty;
    public string TimeReportsApi { get; set; } = string.Empty;
    public string NotificationsApi { get; set; } = string.Empty;
}
