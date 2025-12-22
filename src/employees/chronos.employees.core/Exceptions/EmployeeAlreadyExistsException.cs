using chronos.shared.exceptions;
using System.Net;

namespace chronos.employees.core.Exceptions;

public sealed class EmployeeAlreadyExistsException(string firstName, string lastName)
    : ChronosException(
        $"employee.already_exists",
        $"Employee with name {firstName} {lastName} already exists",
        $"{firstName} {lastName}",
        HttpStatusCode.Conflict);
