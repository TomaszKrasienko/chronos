# TimeLogAutomaticallyAccepted Event

## Event Flow

```mermaid
flowchart LR
    subgraph Source["Source Service"]
        A[Contracts]
    end
    
    subgraph RabbitMQ["RabbitMQ"]
        B[Exchange: chronos_contracts_core]
        C[Routing Key: time_log_automatically_accepted]
        D[Queue: time_logs_time_log_automatically_accepted]
    end
    
    subgraph Target["Target Service"]
        E[TimeLogs]
    end
    
    A -->|publishes| B
    B -->|routes via| C
    C -->|delivers to| D
    D -->|consumed by| E
```

## Configuration

| Property | Value |
|----------|-------|
| Exchange | `chronos_contracts_core` |
| Routing Key | `time_log_automatically_accepted` |
| Queue | `time_logs_time_log_automatically_accepted` |
| Pattern | Durable (IsTemporary: false) |

## Event Payload

```csharp
public sealed record TimeLogAutomaticallyAccepted(
    TimeLogId TimeLogId) : IMessage;
```

| Field | Type | Description |
|-------|------|-------------|
| TimeLogId | TimeLogId | Strongly-typed identifier of the accepted time log |

## Consumer Behavior

The TimeLogs service transitions the time log from `WaitingForAcceptation` state to `Accepted` state, recording the acceptance timestamp and acceptor (supervisor).
