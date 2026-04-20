# EmployeeDeleted Event

## Event Flow

```mermaid
flowchart LR
    subgraph Source["Source Service"]
        A[Employees]
    end
    
    subgraph RabbitMQ["RabbitMQ"]
        B[Exchange: chronos_employees_core]
        C[Routing Key: employee_deleted]
        D[Queue: contracts_employees_deleted]
    end
    
    subgraph Target["Target Service"]
        E[Contracts]
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
| Routing Key | `employee_deleted` |
| Queue | `contracts_employees_deleted` |
| Pattern | Fanout (IsTemporary: true) for publisher, Durable for consumer |

## Event Payload

```csharp
public sealed record EmployeeDeleted(Ulid Id) : IMessage;
```

| Field | Type | Description |
|-------|------|-------------|
| Id | Ulid | Unique identifier of the deleted employee |

## Consumer Behavior

When consumed by the Contracts service, it removes the deleted employee from all contracts they were assigned to.
