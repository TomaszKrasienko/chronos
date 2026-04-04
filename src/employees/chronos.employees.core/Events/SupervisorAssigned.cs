using chronos.shared.messaging;

namespace chronos.employees.core.Events;

public sealed record SupervisorAssigned(
    Ulid EmployeeId,
    Ulid SupervisorId) : IMessage;