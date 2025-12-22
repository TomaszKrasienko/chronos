using System.Net.Http.Json;
using chronos.ui.Models;

namespace chronos.ui.Services;

public sealed class TimeLogService(IHttpClientFactory httpClientFactory) : ITimeLogService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("TimeLoggersAPI");

    public async Task<List<TimeLog>> GetAllAsync()
    {
        try
        {
            // TODO: Implement GET /api/time-loggers endpoint in API
            var timeLogs = await _httpClient.GetFromJsonAsync<List<TimeLog>>("/api/time-loggers");
            return timeLogs ?? new List<TimeLog>();
        }
        catch (HttpRequestException)
        {
            // Return mock data if API is not available
            return GetMockTimeLogs();
        }
    }

    public async Task<List<TimeLog>> GetByEmployeeIdAsync(string employeeId)
    {
        try
        {
            var timeLogs = await _httpClient.GetFromJsonAsync<List<TimeLog>>($"/api/time-loggers?employeeId={employeeId}");
            return timeLogs ?? new List<TimeLog>();
        }
        catch (HttpRequestException)
        {
            // Return mock data if API is not available
            return GetMockTimeLogs().Where(t => t.EmployeeId == employeeId).ToList();
        }
    }

    public async Task<string> CreateAsync(CreateTimeLogRequest request, string employeeId)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/time-loggers");
        requestMessage.Headers.Add("X-Employee-Id", employeeId);
        requestMessage.Content = JsonContent.Create(request);

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        // API returns just the ID as string
        return result.Trim('"');
    }

    public async Task<List<TimeLog>> GetMyTimeLogsAsync(string employeeId)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/time-loggers/my");
        requestMessage.Headers.Add("X-Employee-Id", employeeId);

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TimeLog>>();
        return result ?? new List<TimeLog>();
    }

    public async Task<List<EmployeeTimeLogs>> GetSubordinatesTimeLogsAsync(string supervisorId)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/time-loggers/subordinates");
        requestMessage.Headers.Add("X-Employee-Id", supervisorId);

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<EmployeeTimeLogs>>();
        return result ?? new List<EmployeeTimeLogs>();
    }

    public async Task ApproveAsync(string timeLogId)
    {
        // TODO: Implement PUT /api/time-loggers/{id}/approve endpoint in API
        var response = await _httpClient.PutAsync($"/api/time-loggers/{timeLogId}/approve", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task AcceptAsync(string timeLogId, string supervisorId)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Patch, $"/api/time-loggers/{timeLogId}/accept");
        requestMessage.Headers.Add("X-Employee-Id", supervisorId);

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
    }

    public async Task RejectAsync(string timeLogId, string supervisorId)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Patch, $"/api/time-loggers/{timeLogId}/reject");
        requestMessage.Headers.Add("X-Employee-Id", supervisorId);

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
    }

    private static List<TimeLog> GetMockTimeLogs() => new()
    {
        new TimeLog
        {
            Id = "01JFABCDEFGHIJKLMNOPQRSTUY",
            EmployeeId = "01JFABCDEFGHIJKLMNOPQRSTUW",
            EmployeeName = "Jane Smith",
            TimeSpan = TimeSpan.FromHours(8),
            Topic = "Project X Development",
            Notes = "Working on feature implementation",
            Status = "Pending"
        },
        new TimeLog
        {
            Id = "01JFABCDEFGHIJKLMNOPQRSTUZ",
            EmployeeId = "01JFABCDEFGHIJKLMNOPQRSTUX",
            EmployeeName = "Bob Johnson",
            TimeSpan = TimeSpan.FromHours(6.5),
            Topic = "Code Review",
            Notes = null,
            Status = "Accepted"
        },
        new TimeLog
        {
            Id = "01JFABCDEFGHIJKLMNOPQRST10",
            EmployeeId = "01JFABCDEFGHIJKLMNOPQRSTUW",
            EmployeeName = "Jane Smith",
            TimeSpan = TimeSpan.FromHours(4),
            Topic = "Meeting with client",
            Notes = "Discussed requirements",
            Status = "Accepted"
        }
    };
}
