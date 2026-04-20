using System.Net;
using chronos.shared.kernel.Exceptions;
using chronos.shared.kernel.Identifiers;
using Refit;

namespace chronos.contracts.core.Communication.Sync.Http;

/// <summary>
/// Client for checking employee existence in the Employees service
/// </summary>
public interface IEmployeesClient
{
    /// <summary>
    /// Checks if an employee exists
    /// </summary>
    /// <param name="employeeId">The unique identifier of the employee</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the employee exists, otherwise false</returns>
    Task<bool> DoesEmployeeExistAsync(EmployeeId employeeId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Refit HTTP client for communication with Employees API
/// </summary>
public interface IEmployeeHttpClient
{
    /// <summary>
    /// Gets an employee by ID
    /// </summary>
    [Get("/api/employees/{employeeId}")]
    Task<HttpResponseMessage> GetByIdAsync(Ulid employeeId, CancellationToken cancellationToken = default);
}

internal sealed class EmployeesClient(IEmployeeHttpClient httpClient) : IEmployeesClient
{
    /// <inheritdoc />
    public async Task<bool> DoesEmployeeExistAsync(EmployeeId employeeId, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetByIdAsync(employeeId.Value, cancellationToken);

        return response.StatusCode switch
        {
            HttpStatusCode.OK => true,
            HttpStatusCode.NotFound => false,
            _ => throw new InvalidCommunicationException("employees_communication_failed")
        };
    }
}