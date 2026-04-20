# TimeLogCreated Event

## Event Flow

```mermaid
flowchart LR
    subgraph Source["Source Service"]
        A[TimeLogs]
    end
    
    subgraph RabbitMQ["RabbitMQ"]
        B[Exchange: chronos_time_loggers_core]
        C[Routing Key: time_log_created]
        D[Queue: notifications_time_log_created]
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
| Exchange | `chronos_time_loggers_core` |
| Routing Key | `time_log_created` |
| Queue | `notifications_time_log_created` |
| Pattern | Durable (IsTemporary: false) |

**Note:** This event is configured as a consumer in Notifications but no publisher route is visible in TimeLogs appsettings.json.

## Event Payload (Consumer Side)

```csharp
public sealed record TimeLogCreated(
    Ulid Id,
    TimeSpan TimeSpan,
    Ulid EmployeeId,
    string Topic,
    string? Notes);
```

| Field | Type | Description |
|-------|------|-------------|
| Id | Ulid | Unique identifier of the time log |
| TimeSpan | TimeSpan | Amount of time logged |
| EmployeeId | Ulid | Employee who logged the time |
| Topic | string | Topic/description of the work |
| Notes | string? | Optional additional notes |
