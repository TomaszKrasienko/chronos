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

- Bounded Context: TimeLogs - TODO
  - Aggregate MonthlyTimeReport: Aggregates every time log for contract and user
  - Entity TimeLog (Abstract): Information about logged time. Time is logged by employee. TimeLog should know about employeeId, amount of hours and contract - at the beginning contract will be replaced by topic. TimeLog has to have a place for notes.
    - Implementations of TimeLog
      - AcceptedTimeLog - after acceptance from supervisor or after automatic acceptance, with additional field AcceptedBy
      - RejectedTimeLog - after rejection, with additional fields Reason, RejectedBy
      - WaitingForAcceptation - with additional field - SupervisorId
  
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
- `chronos.{name}.api` - ASP.NET Core Web API (endpoints, DTOs)
- `chronos.{name}.core` - Business logic with subdirectories:
  - `Domain/` - OOP domain models with subdirectories:
    - `Identifiers/` - Strongly-typed IDs
    - `Rules/` - Business rules implementing `IBusinessRule`
    - `ValueObjects/` - Value objects
  - `DAL/` - MongoDB with EF Core, one DbContext per service (e.g., `EmployeesDbContext`)
  - `Events/` - Integration events for event-driven architecture
  - `Communication/` - Async (RabbitMQ) and Sync (gRPC) communication
  - `Services/` - Application services
  - `Configuration/` - DI extensions

### Configuration Pattern
Each feature in Core projects has a `Configuration/` directory with extension methods:
```csharp
public static IServiceCollection Add{FeatureName}(this IServiceCollection services, IConfiguration configuration)
```
Main entry point is `AddCore()` which chains: `AddDal()`, `AddCommunication()`, etc.

### Shared Libraries
- `chronos.shared.kernel` - DDD building blocks (IEntityId, Entity, AggregateRoot, ValueObject, IBusinessRule, DomainException)
- `chronos.shared.configuration` - Configuration utilities
- `chronos.shared.exceptions` - Exception handling middleware
- `chronos.shared.identity-context` - Employee context from HTTP headers
- `chronos.shared.messaging` - Messaging abstractions
- `chronos.shared.messaging.rabbit-mq` - RabbitMQ implementation

### Domain Modeling
- Strongly-typed IDs: `readonly record struct` implementing `IEntityId` with `New()` factory method
- Aggregates inherit from `AggregateRoot<TId>`
- Entities inherit from `Entity<TId>`
- Value objects inherit from `ValueObject`
- Business rules: `sealed` classes implementing `IBusinessRule` with `Code` property and `IsBroken()` method, placed in `Rules/` folder
- Validation: use `CheckRule(new SomeRule(...))` in Entity/ValueObject - throws `DomainException` with error code
- Use primary constructors where possible
- Delete operations should be idempotent (no exception when entity not found)
- One employee can have only one assignment per contract (no overlapping periods)

## Technologies

- .NET 9, MongoDB with EF Core, RabbitMQ
- Scrutor for decorator pattern in DI
- Swashbuckle for Swagger UI
- YARP for reverse proxy
- CRON Jobs executed by hangfire

## Code Conventions

- Async methods must have `Async` suffix
- Async event handlers must include `CancellationToken cancellationToken = default` as last parameter
- Domain models use OOP with business logic encapsulated
- Uses Ulid for identifiers (not Guid)
- Project settings: `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`

### Namespaces
Extension class should have namespace of extended object - e.g. IServiceCollection 

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

### Test Libraries
- xUnit as test framework
- Shouldly for assertions
- Factories pattern for test data creation

## Git Workflow
- Main branch: `main`
- Development branch: `develop`
- Create feature branches from `develop`