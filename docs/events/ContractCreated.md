# ContractCreated Event

## Event Flow

```mermaid
flowchart LR
    subgraph Source["Source Service"]
        A[Contracts]
    end
    
    subgraph RabbitMQ["RabbitMQ"]
        B[Exchange: chronos_contracts_core]
        C[Routing Key: contract_created]
        D[Temporary Queue]
    end
    
    subgraph Target["Target Service"]
        E[Self / Fanout]
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
| Routing Key | `contract_created` |
| Queue | Temporary (auto-generated) |
| Pattern | Fanout (IsTemporary: true) |

## Event Payload

```csharp
public sealed record ContractCreated(
    Ulid ContractId,
    string CompanyName,
    DateOnly AssignmentDate,
    DateOnly? ClosingDate) : IMessage;
```

| Field | Type | Description |
|-------|------|-------------|
| ContractId | Ulid | Unique identifier of the created contract |
| CompanyName | string | Name of the company |
| AssignmentDate | DateOnly | Contract assignment date |
| ClosingDate | DateOnly? | Optional contract closing date (null if active) |
