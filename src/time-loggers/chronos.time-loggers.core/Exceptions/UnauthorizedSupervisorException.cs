using chronos.shared.exceptions;
using System.Net;

namespace chronos.time_loggers.core.Exceptions;

public sealed class UnauthorizedSupervisorException(Ulid supervisorId, Ulid employeeId)
    : ChronosException(
        "supervisor.unauthorized",
        $"Supervisor {supervisorId} is not authorized to manage time logs for employee {employeeId}",
        supervisorId.ToString(),
        HttpStatusCode.Forbidden);
