using System.Net.Http.Json;
using chronos.ui.Models;

namespace chronos.ui.Services;

public sealed class TimeReportService(IHttpClientFactory httpClientFactory) : ITimeReportService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("TimeReportsAPI");

    public async Task<TimeReport?> GetByEmployeeIdAsync(string employeeId)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "time-reports");
        requestMessage.Headers.Add("X-Employee-Id", employeeId);

        var response = await _httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<TimeReport>();
        return result;
    }

    public async Task<TimeReport?> GetByEmployeeAndPeriodAsync(string employeeId, string period)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<TimeReport>(
                $"time-reports?employeeId={employeeId}&period={period}");
        }
        catch (HttpRequestException)
        {
            // Return mock data if API is not available
            return GetMockTimeReports()
                .FirstOrDefault(r => r.EmployeeId == employeeId && r.Period == period);
        }
    }

    public async Task GenerateReportFileAsync(string employeeId)
    {
        await _httpClient.PostAsync($"time-reports/files/{employeeId}", null);
    }

    private static List<TimeReport> GetMockTimeReports() => new()
    {
        new TimeReport
        {
            Id = "01JFABCDEFGHIJKLMNOPQRST01",
            EmployeeId = "01JFABCDEFGHIJKLMNOPQRSTUW",
            EmployeeName = "Jane Smith",
            Period = "December 2024",
            Summary = "156:30:00",
            AcceptedCount = 18,
            RejectedCount = 0
        },
        new TimeReport
        {
            Id = "01JFABCDEFGHIJKLMNOPQRST02",
            EmployeeId = "01JFABCDEFGHIJKLMNOPQRSTUX",
            EmployeeName = "Bob Johnson",
            Period = "December 2024",
            Summary = "168:00:00",
            AcceptedCount = 21,
            RejectedCount = 1
        },
        new TimeReport
        {
            Id = "01JFABCDEFGHIJKLMNOPQRST03",
            EmployeeId = "01JFABCDEFGHIJKLMNOPQRSTUV",
            EmployeeName = "John Doe",
            Period = "December 2024",
            Summary = "172:00:00",
            AcceptedCount = 22,
            RejectedCount = 0
        }
    };
}
