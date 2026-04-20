using System.Net;

namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Exception thrown when an employee has no supervisor assigned.
/// </summary>
public sealed class EmployeeHasNoSupervisorException(
    string[]? @params = null) : ChronosException("employee_has_no_supervisor", @params)
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
}
