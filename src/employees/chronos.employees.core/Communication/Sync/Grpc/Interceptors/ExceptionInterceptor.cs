using chronos.employees.core.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace chronos.employees.core.Communication.Sync.Grpc.Interceptors;

internal sealed class ExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (EmployeeNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }
}
