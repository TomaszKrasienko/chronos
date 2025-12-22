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

    public async Task<EmployeeDto?> GetByIdAsync(Ulid employeeId, CancellationToken cancellationToken)
    {
        var request = new GetByIdRequest
        {
            Id = employeeId.ToString()
        };

        try
        {
            var response = await client.GetByIdAsync(request, cancellationToken: cancellationToken);

            return new EmployeeDto(
                Ulid.Parse(response.Id),
                response.FirstName,
                response.LastName,
                string.IsNullOrEmpty(response.SupervisorId) ? null : Ulid.Parse(response.SupervisorId));
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
        catch (RpcException ex)
        {
            RcpExceptionHandler.HandleException(ex, logger);
            throw;
        }
    }

    public async Task<IReadOnlyCollection<EmployeeDto>> GetSubordinatesAsync(
        Ulid supervisorId,
        CancellationToken cancellationToken)
    {
        var request = new GetSubordinatesRequest
        {
            SupervisorId = supervisorId.ToString()
        };

        try
        {
            var response = await client.GetSubordinatesAsync(request, cancellationToken: cancellationToken);

            return response.Employees.Select(e => new EmployeeDto(
                Ulid.Parse(e.Id),
                e.FirstName,
                e.LastName,
                string.IsNullOrEmpty(e.SupervisorId) ? null : Ulid.Parse(e.SupervisorId)
            )).ToList();
        }
        catch (RpcException ex)
        {
            RcpExceptionHandler.HandleException(ex, logger);
            throw;
        }
    }
}