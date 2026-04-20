using System.Net;
using System.Net.Http.Json;
using chronos.time_logs.core.Communication.Sync.Http.DTOs;
using chronos.time_logs.core.Communication.Sync.Http.Exceptions;
using Microsoft.Extensions.Logging;

namespace chronos.time_logs.core.Communication.Sync.Http.Clients;

internal sealed class HttpContractsClient(
    HttpClient httpClient,
    ILogger<HttpContractsClient> logger) : IContractsClient
{
    public async Task<bool> IsEmployeeAssignedToContractAsync(
        Ulid contractId,
        Ulid employeeId,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync(
            $"/api/contracts/{contractId}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        if (!response.IsSuccessStatusCode)
        {
            HttpExceptionHandler.HandleException(response, logger);
            return false;
        }

        var contract = await response.Content.ReadFromJsonAsync<ContractResponseDto>(cancellationToken);

        if (contract is null)
        {
            return false;
        }

        return contract.Employees.Any(e => e.EmployeeId == employeeId.ToString());
    }
}
