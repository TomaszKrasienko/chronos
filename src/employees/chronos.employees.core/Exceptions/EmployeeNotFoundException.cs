using chronos.shared.exceptions;
using System.Net;

namespace chronos.employees.core.Exceptions;

public sealed class EmployeeNotFoundException(Ulid employeeId)
    : ChronosException(
        $"employee.not_found",
        $"Employee with ID {employeeId} not found",
        employeeId.ToString(),
        HttpStatusCode.NotFound);