using chronos.shared.messaging;

namespace chronos.employees.core.Events;

public sealed record EmployeeDeleted(Ulid Id) : IMessage;
