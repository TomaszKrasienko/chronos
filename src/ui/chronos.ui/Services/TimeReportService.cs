using System.Net.Http.Json;
using chronos.ui.Models;

namespace chronos.ui.Services;

public sealed class TimeReportService(IHttpClientFactory httpClientFactory) : ITimeReportService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("TimeReportsAPI");

    public async Task<List<TimeReport>> GetAllAsync()
    {
        try
        {
            // TODO: Implement GET /api/time-reports endpoint in API
            var reports = await _httpClient.GetFromJsonAsync<List<TimeReport>>("/api/time-reports");
            return reports ?? new List<TimeReport>();
        }
        catch (HttpRequestException)
        {
            // Return mock data if API is not available
            return GetMockTimeReports();
        }
    }

    public async Task<TimeReport?> GetByEmployeeAndPeriodAsync(string employeeId, string period)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<TimeReport>(
                $"/api/time-reports?employeeId={employeeId}&period={period}");
        }
        catch (HttpRequestException)
        {
            // Return mock data if API is not available
            return GetMockTimeReports()
                .FirstOrDefault(r => r.EmployeeId == employeeId && r.Period == period);
        }
    }

    private static List<TimeReport> GetMockTimeReports() => new()
    {
        new TimeReport
        {
            EmployeeId = "01JFABCDEFGHIJKLMNOPQRSTUW",
            EmployeeName = "Jane Smith",
            Period = "December 2024",
            TotalHours = 156.5m,
            AcceptedEntries = 18,
            PendingEntries = 2,
            RejectedEntries = 0
        },
        new TimeReport
        {
            EmployeeId = "01JFABCDEFGHIJKLMNOPQRSTUX",
            EmployeeName = "Bob Johnson",
            Period = "December 2024",
            TotalHours = 168.0m,
            AcceptedEntries = 21,
            PendingEntries = 0,
            RejectedEntries = 1
        },
        new TimeReport
        {
            EmployeeId = "01JFABCDEFGHIJKLMNOPQRSTUV",
            EmployeeName = "John Doe",
            Period = "December 2024",
            TotalHours = 172.0m,
            AcceptedEntries = 22,
            PendingEntries = 0,
            RejectedEntries = 0
        }
    };
}
