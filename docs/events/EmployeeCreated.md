# EmployeeCreated Event

## Event Flow

```mermaid
flowchart LR
    subgraph Source["Source Service"]
        A[Employees]
    end
    
    subgraph RabbitMQ["RabbitMQ"]
        B[Exchange: chronos_employees_core]
        C[Routing Key: employee_created]
        D[Queue: notifications_employee_created]
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
| Routing Key | `employee_created` |
| Queue | `notifications_employee_created` |
| Pattern | Fanout (IsTemporary: true) for publisher, Durable for consumer |

## Event Payload

```csharp
public sealed record EmployeeCreated(
    Ulid Id,
    string FirstName,
    string LastName,
    string Email,
    Ulid? SupervisorId) : IMessage;
```

| Field | Type | Description |
|-------|------|-------------|
| Id | Ulid | Unique identifier of the created employee |
| FirstName | string | Employee's first name |
| LastName | string | Employee's last name |
| Email | string | Employee's email address |
| SupervisorId | Ulid? | Optional supervisor identifier |
