using chronos.employees.core.Exceptions;
using chronos.employees.core.Services;
using Grpc.Core;

namespace chronos.employees.core.Communication.Sync.Grpc;

internal sealed class EmployeesService(
    IEmployeeService employeeService) : EmployeeService.EmployeeServiceBase
{
    public override async Task<DoesExistResponse> DoesExist(
        DoesExistRequest request,
        ServerCallContext context)
    {
        if (!Ulid.TryParse(request.Id, out var employeeId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid employee ID format"));
        }

        var employee = await employeeService.GetByIdAsync(
            employeeId,
            context.CancellationToken);

        return new DoesExistResponse
        {
            Value = employee is not null
        };
    }

    public override async Task<GetByIdResponse> GetById(
        GetByIdRequest request,
        ServerCallContext context)
    {
        if (!Ulid.TryParse(request.Id, out var employeeId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid employee ID format"));
        }

        var employee = await employeeService.GetByIdAsync(
            employeeId,
            context.CancellationToken);

        if (employee is null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        return new GetByIdResponse
        {
            Id = employee.Id.ToString(),
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            SupervisorId = employee.SupervisorId?.ToString() ?? string.Empty
        };
    }

    public override async Task<GetSubordinatesResponse> GetSubordinates(
        GetSubordinatesRequest request,
        ServerCallContext context)
    {
        if (!Ulid.TryParse(request.SupervisorId, out var supervisorId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid supervisor ID format"));
        }

        var subordinates = await employeeService.GetSubordinatesAsync(
            supervisorId,
            context.CancellationToken);

        var response = new GetSubordinatesResponse();

        foreach (var employee in subordinates)
        {
            response.Employees.Add(new GetByIdResponse
            {
                Id = employee.Id.ToString(),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                SupervisorId = employee.SupervisorId?.ToString() ?? string.Empty
            });
        }

        return response;
    }
}