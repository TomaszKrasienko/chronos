# CLAUDE.md
This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview
- Chronos is a microservices-based time tracking system built with .NET 9 and Blazor WebAssembly.

## Domain Architecture

### Bounded contexts
- Bounded context: Employee
  - Aggregate Employee: Base information about person: FirstName, LastName, Email, Supervisor

- Bounded Context: Contracts
  - Aggregate Contract: Company details (VO), contract period (assignment date, closing date), list of employees
  - Entity ContractEmployee: EmployeeId (Ulid reference), AssignmentPeriod (VO: from/to DateOnly), AllocatedHours

- Bounded Context: TimeLogs
  - Aggregate MonthlyTimeReport: Aggregates every time log for contract and user per month
  - Entity TimeLog (Abstract): Information about logged time with EmployeeId, ContractId, Hours, Topic, Notes
    - WaitingForAcceptation - initial state with SupervisorId
    - AcceptedTimeLog - after acceptance with AcceptedBy, AcceptedAt
    - RejectedTimeLog - after rejection with Reason, RejectedBy, RejectedAt
  
### Additional modules
- Notifications
- ReverseProxy
- Jobs

### Events
- TimeLogCreated - After creation of WaitingForAcceptation
  Executes process of pre-acceptation - process is in Contracts and it checks that hours are not exceeded.
- TimeLogAutomaticlyRejected - After creation of WaitingForAcceptation
  Executes process of pre-acceptation - process is in Contracts and it checks that hours are exceeded
- TimeLogAccepted - After handly supervisor acceptation 
- TimeLogRejected - After handly supervisor rejection 

## Build & Run Commands

```bash
# Build entire solution
dotnet build

# Run all tests
dotnet test

# Run individual service (from repo root)
dotnet run --project src/employees/chronos.employees.api

# Start infrastructure (MongoDB + RabbitMQ)
cd chronos_scripts/chronos_docker && ./wipe_and_run_env.sh

# Build Docker image for a service
cd chronos_scripts/builds && ./employees.sh
```

## Architecture

### Microservice Structure
Every microservice follows this pattern in `src/{name}/`:
- `chronos.{name}.api` - ASP.NET Core Web API (endpoints only)
- `chronos.{name}.core` - Business logic with subdirectories:
  - `Domain/` - OOP domain models with subdirectories:
    - `Events/` - Domain events implementing `IDomainEvent`
    - `Rules/` - Business rules implementing `IBusinessRule`
    - `ValueObjects/` - Value objects
  - `DTOs/` - Request and Response DTOs with subdirectories:
    - `Requests/` - Request DTOs
    - `Responses/` - Response DTOs (use simple types like `string` for IDs, not strongly-typed IDs)
    - `Mappers/` - Extension classes for mapping domain objects to DTOs
  - `DAL/` - MongoDB with EF Core, one DbContext per service (e.g., `EmployeesDbContext`)
  - `Events/` - Integration events for event-driven architecture (sealed records with strongly-typed IDs)
  - `Communication/` - Async (RabbitMQ) and Sync (gRPC) communication
  - `Services/` - Application services
  - `Configuration/` - DI extensions

### Configuration Pattern
Each feature in Core projects has a `Configuration/` directory with extension methods:
```csharp
public static IServiceCollection Add{FeatureName}(this IServiceCollection services, IConfiguration configuration)
```
Main entry point is `AddCore()` which chains: `AddDal()`, `AddCommunication()`, etc.

### Application Services
- Split interfaces into `IRead{Name}Service` and `IWrite{Name}Service` for query/command separation
- Single implementation class `{Name}Service` implements both interfaces
- Registration in DI:
```csharp
.AddScoped<IReadContractsService, ContractsService>()
.AddScoped<IWriteContractsService, ContractsService>();
```
- Use generic exceptions from `chronos.shared.kernel/Exceptions/`: `NotFoundException`, `NotUniqueException`, etc.
- `DomainException` is only for domain layer (entities, value objects) - never use in services
- If no matching exception exists, create a new one following the pattern:
```csharp
public sealed class EmployeeHasNoSupervisorException(
    string[]? @params = null) : ChronosException("employee_has_no_supervisor", @params)
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
}
```

### Shared Libraries
- `chronos.shared.kernel` - DDD building blocks (IEntityId, Entity, AggregateRoot, ValueObject, IBusinessRule, IDomainEvent, DomainException) and strongly-typed identifiers in `Identifiers/` folder
- `chronos.shared.configuration` - Configuration utilities
- `chronos.shared.exceptions` - Exception handling middleware
- `chronos.shared.identity-context` - Employee context from HTTP headers
- `chronos.shared.messaging` - Messaging abstractions
- `chronos.shared.messaging.rabbit-mq` - RabbitMQ implementation

### Domain Modeling
- Strongly-typed IDs: `readonly record struct` implementing `IEntityId` with `New()` factory method, placed in `chronos.shared.kernel/Identifiers/` for cross-module reuse (e.g., in events)
- Aggregates inherit from `AggregateRoot<TId>`
- Entities inherit from `Entity<TId>`
- Value objects inherit from `ValueObject`
- Business rules: `sealed` classes implementing `IBusinessRule` with `Code` property and `IsBroken()` method, placed in `Rules/` folder
- Validation: use `CheckRule(new SomeRule(...))` in Entity/ValueObject - throws `DomainException` with error code
- Use primary constructors where possible
- Delete operations should be idempotent (no exception when entity not found)
- One employee can have only one assignment per contract (no overlapping periods)

### Domain Events
- Domain events are `sealed record` types implementing `IDomainEvent`, placed in `Domain/Events/` folder
- Every domain method in aggregates should raise domain events using `AddDomainEvent(new SomeEvent(...))`
- Events contain all relevant data (IDs, values) - not references to domain objects
- Domain events are NOT persisted to database - they are dispatched after successful save operation
- Use `ClearDomainEvents()` after publishing events

## Documentation

### Event Communication Diagrams
- After adding or modifying integration events, generate Mermaid diagrams using the `mermaid-docs` sub-agent
- Agent definition: `.claude/agents/mermaid-docs.md`
- Output location: `docs/events/{EventName}.md`
- Each diagram shows: Source Service → Exchange → Routing Key → Queue → Target Service
- Run with prompt: "Generate event documentation diagrams" or "Update diagram for {EventName}"

### XML Summary Documentation
Use the `xml-docs` sub-agent to add missing XML documentation. Agent definition: `.claude/agents/xml-docs.md`

#### Convention by Type
| Type | Documentation Style |
|------|---------------------|
| Interfaces | Full documentation: `<summary>`, `<param>`, `<returns>` |
| Implementations | `/// <inheritdoc />` for interface methods, `<summary>` for class |
| Abstract/Base classes | Full documentation with `<typeparam>` |
| Aggregates/Entities | `<summary>` for class and public methods |
| Value Objects | `<summary>` for class and factory methods |
| Business Rules | `<summary>` describing what the rule validates |
| DTOs (records) | `<summary>` for record, `<param>` for each property |
| Events | `<summary>` describing when event is raised |

#### Writing Style
- Be concise - one sentence for simple members
- Start with verb: "Creates...", "Gets...", "Validates..."
- Use "The" for parameters: "The contract identifier"
- Reference types with `<see cref="TypeName"/>`
- CancellationToken: always "The cancellation token."

#### Example
```csharp
// Interface - full documentation
/// <summary>
/// Service for writing contract data.
/// </summary>
public interface IWriteContractsService
{
    /// <summary>
    /// Creates a new contract with company details.
    /// </summary>
    /// <param name="companyName">The name of the company.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created contract identifier.</returns>
    Task<ContractId> CreateContractAsync(string companyName, CancellationToken cancellationToken = default);
}

// Implementation - inheritdoc
/// <summary>
/// Service for managing contracts.
/// </summary>
internal sealed class ContractsService : IWriteContractsService
{
    /// <inheritdoc />
    public async Task<ContractId> CreateContractAsync(...) { }
}
```

## Technologies

- .NET 9, MongoDB with EF Core, RabbitMQ
- Scrutor for decorator pattern in DI
- Swashbuckle for Swagger UI
- YARP for reverse proxy
- CRON Jobs executed by hangfire

## Code Conventions

- Prefer Chronos exceptions (inheriting from `ChronosException`) over standard .NET exceptions - they are properly handled by exception middleware and return appropriate HTTP status codes
- Use collection expressions `[]` for new collections: `List<Contract> contracts = [contract1, contract2]` instead of `new List<Contract> { ... }`
- Async methods must have `Async` suffix
- Async event handlers must include `CancellationToken cancellationToken = default` as last parameter
- Domain models use OOP with business logic encapsulated
- Uses Ulid for identifiers (not Guid)
- Project settings: `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`
- XML summary: when referencing types/methods, use `<see cref="TypeName"/>` instead of plain text
- Multi-argument formatting: when more than one argument, place each on a new line:
```csharp
public sealed record AssignEmployeeRequestDto(
    Ulid EmployeeId,
    DateOnly From,
    DateOnly? To,
    int AllocatedHours);
```

### Namespaces
Extension class should have namespace of extended object - e.g. IServiceCollection

### Mappers
- Place mappers in `DTOs/Mappers/` folder as static extension classes
- Mapper extension class namespace must match the namespace of the extended class (e.g., `chronos.contracts.core.Domain` for `Contract` extensions)
- Public method: `ToDto()` extension method on the aggregate
- Private methods for mapping child entities/value objects 

## Testing

### Test Project Structure
```
tests/
├── shared/chronos.tests.shared/
│   └── Factories/           # Shared test factories
└── {name}/chronos.{name}.core.tests/
    └── Domain/
        ├── {Aggregate}/     # Folder per aggregate (e.g., Contracts/)
        │   ├── CreateTests.cs
        │   ├── {Method}Tests.cs
        └── ValueObjects/
            └── {ValueObject}/  # Folder per VO (e.g., AssignmentPeriod/)
                ├── CreateTests.cs
                └── EqualityTests.cs
```

### Test Conventions
- Use Shouldly for assertions
- Naming convention: `Given{State}_When{Action}_Then{Result}`
- For DomainException tests: `Then{Result}WithCode_{error_code}`
- All test classes must be `sealed`
- Separate Arrange / Act / Assert sections with comments
- Factories in shared project with private default values
- **Class structure:** Place constructor and fields at the bottom of the test class, test methods first

### Service Tests
- Use NSubstitute for mocking dependencies
- Place in `Services/{ServiceName}Tests/` folder with one file per method
- Structure: `{MethodName}Tests.cs` (e.g., `CreateContractAsyncTests.cs`)
- Add `<InternalsVisibleTo Include="DynamicProxyGenAssembly2" />` to core project for mocking internal types (no public key needed)

### Test Libraries
- xUnit as test framework
- Shouldly for assertions
- NSubstitute for mocking (service tests)
- Factories pattern for test data creation

## Git Workflow
- Main branch: `main`
- Development branch: `develop`
- Create feature branches from `develop`