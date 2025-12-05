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
}