using chronos.employees;
using chronos.time_loggers.core.Communication.Sync.Grpc.Exceptions;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace chronos.time_loggers.core.Communication.Sync.Grpc;

public sealed class GrpcEmployeesClient(
    EmployeeService.EmployeeServiceClient client,
    ILogger<GrpcEmployeesClient> logger) : IEmployeesClient
{
    public async Task<bool> AnyAsync(Ulid employeeId, CancellationToken cancellationToken)
    {
        var request = new DoesExistRequest
        {
            Id = employeeId.ToString()
        };

        try
        {
            var response = await client.DoesExistAsync(request, cancellationToken: cancellationToken);
            return response.Value;
        }
        catch (RpcException ex)
        {
            RcpExceptionHandler.HandleException(ex, logger);
            throw;
        }
    }
}