using System.Net;
using chronos.shared.exceptions;

namespace chronos.notifications.core.Exceptions;

public sealed class ContactAlreadyExistsException(Ulid employeeId)
    : ChronosException(
        $"contact.already_exists",
        $"Contact for employee with ID {employeeId} already exists",
        employeeId.ToString(),
        HttpStatusCode.Conflict);
