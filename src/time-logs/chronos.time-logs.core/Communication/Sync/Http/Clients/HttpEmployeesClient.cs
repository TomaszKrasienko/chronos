using System.Net;
using System.Net.Http.Json;
using chronos.time_logs.core.Communication.Sync.Http.DTOs;
using chronos.time_logs.core.Communication.Sync.Http.Exceptions;
using Microsoft.Extensions.Logging;

namespace chronos.time_logs.core.Communication.Sync.Http.Clients;

internal sealed class HttpEmployeesClient(
    HttpClient httpClient,
    ILogger<HttpEmployeesClient> logger) : IEmployeesClient
{
    public async Task<bool> AnyAsync(Ulid employeeId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(
            $"/api/employees/{employeeId}",
            cancellationToken);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return true;
            case HttpStatusCode.NotFound:
                return false;
        }

        if (!response.IsSuccessStatusCode)
        {
            HttpExceptionHandler.HandleException(response, logger);
        }

        return false;
    }

    public async Task<EmployeeDto?> GetByIdAsync(Ulid employeeId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(
            $"/api/employees/{employeeId}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            HttpExceptionHandler.HandleException(response, logger);
            return null;
        }

        var dto = await response.Content.ReadFromJsonAsync<EmployeeResponseDto>(cancellationToken);

        if (dto is null)
        {
            return null;
        }

        return new EmployeeDto(
            Ulid.Parse(dto.Id),
            dto.FirstName,
            dto.LastName,
            string.IsNullOrEmpty(dto.SupervisorId) ? null : Ulid.Parse(dto.SupervisorId));
    }

    public async Task<IReadOnlyCollection<EmployeeDto>> GetSubordinatesAsync(
        Ulid supervisorId,
        CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(
            $"/api/employees/{supervisorId}/subordinates",
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            HttpExceptionHandler.HandleException(response, logger);
            return [];
        }

        var subordinateEmployees = await response
            .Content
            .ReadFromJsonAsync<List<EmployeeResponseDto>>(cancellationToken);

        if (subordinateEmployees is null || subordinateEmployees.Count == 0)
        {
            return [];
        }

        return subordinateEmployees.Select(dto => new EmployeeDto(
            Ulid.Parse(dto.Id),
            dto.FirstName,
            dto.LastName,
            string.IsNullOrEmpty(dto.SupervisorId) ? null : Ulid.Parse(dto.SupervisorId)))
            .ToList();
    }
}