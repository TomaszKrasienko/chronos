# SupervisorAssigned Event

## Event Flow

```mermaid
flowchart LR
    subgraph Source["Source Service"]
        A[Employees]
    end
    
    subgraph RabbitMQ["RabbitMQ"]
        B[Exchange: chronos_employees_core]
        C[Routing Key: supervisor_assigned]
        D[Queue: notifications_supervisor_assigned]
    end
    
    subgraph Target["Target Service"]
        E[Notifications]
    end
    
    A -->|publishes| B
    B -->|routes via| C
    C -->|delivers to| D
    D -->|consumed by| E
```

## Configuration

| Property | Value |
|----------|-------|
| Exchange | `chronos_employees_core` |
| Routing Key | `supervisor_assigned` |
| Queue | `notifications_supervisor_assigned` |
| Pattern | Durable (IsTemporary: false) |

## Event Payload

```csharp
public sealed record SupervisorAssigned(
    Ulid EmployeeId,
    Ulid SupervisorId) : IMessage;
```

| Field | Type | Description |
|-------|------|-------------|
| EmployeeId | Ulid | Unique identifier of the employee |
| SupervisorId | Ulid | Unique identifier of the assigned supervisor |
