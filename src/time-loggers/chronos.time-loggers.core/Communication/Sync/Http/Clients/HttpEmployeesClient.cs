using System.Net;
using chronos.time_loggers.core.Communication.Sync.Http.Exceptions;
using Microsoft.Extensions.Logging;

namespace chronos.time_loggers.core.Communication.Sync.Http.Clients;

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
}