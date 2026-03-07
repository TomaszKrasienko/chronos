using System.Net.Http.Json;
using chronos.ui.Models;

namespace chronos.ui.Services;

public sealed class EmployeeService(
    ILogger<EmployeeService> logger,
    IHttpClientFactory httpClientFactory) : IEmployeeService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("EmployeesAPI");

    public async Task<IReadOnlyCollection<Employee>> GetAllAsync()
    {
        try
        {
            // TODO: Implement GET /employees endpoint in API
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>("employees");
            return employees ?? new List<Employee>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, ex.Message);
            return [];
        }
    }

    public async Task<Employee?> GetByIdAsync(string id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Employee>($"employees/{id}");
        }
        catch (HttpRequestException ex )
        {
            logger.LogError(ex, ex.Message);
            return null;
        }
    }

    public async Task<string> CreateAsync(CreateEmployeeRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("employees", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        // API returns just the ID as string
        return result.Trim('"');
    }

    public async Task AssignSupervisorAsync(string employeeId, string supervisorId)
    {
        var response = await _httpClient.PatchAsync(
            $"employees/{employeeId}/supervisors/{supervisorId}",
            null);
        response.EnsureSuccessStatusCode();
    }
}
