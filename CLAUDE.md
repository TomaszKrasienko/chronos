# CLAUDE.md
This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview
- Chronos is a microservices-based time tracking system built with .NET 9 and Blazor WebAssembly.
- Employees submit time-logs that supervisors approve/reject.
- Every time-log has to have a project.
- Time reports aggregate and summarize work hours.

## Domain Architecture

### Bounded contexts
- Bounded context: Employee
  - Aggregate Employee: Base information about person: FirstName, LastName, Email, Supervisor

- Bounded Context: Contracts - TODO
  - Aggregate Contract: Base information about working hours per month grouped by projects. Also information about contract - assignment date, closing date, company name, contract supervisor
  - Entity: Employee: List of employees assigned to contract. In assignment there are hours of employee.

- Time
  - Aggregate MonthlyTimeReport: Aggregates every time log for contract
  - Entity TimeLog (Abstract): Information about logged time. Time is logged by employee. TimeLog should know about employeeId, amount of hours and contract - at the beginning contract will be replaced by topic. TimeLog has to have a place for notes.
    - Implementations of TimeLog
      - AcceptedTimeLog - after acceptance from supervisor or after automatic acceptance, with additional field AcceptedBy
      - RejectedTimeLog - after rejection, with additional fields Reason, RejectedBy
      - WaitingForAcceptation - with additional field - SupervisorId
  

### Events
- TimeLogCreated - After creation of WaitingForAcceptation
  Executes process of pre-acceptation - process is in Contracts and it checks that hours are not exceeded.
- TimeLogAutomaticlyRejected
- TimeLogAccepted
- TimeLogRejected

## Build & Run Commands

```bash
# Build entire solution
dotnet build

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
  - `Domain/` - OOP domain models
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
- `chronos.shared.configuration` - Configuration utilities
- `chronos.shared.exceptions` - Exception handling middleware
- `chronos.shared.identity-context` - Employee context from HTTP headers
- `chronos.shared.messaging` - Messaging abstractions
- `chronos.shared.messaging.rabbit-mq` - RabbitMQ implementation

## Technologies

- .NET 9, MongoDB with EF Core, RabbitMQ
- Scrutor for decorator pattern in DI
- Swashbuckle for Swagger UI
- YARP for reverse proxy

## Code Conventions

- Async methods must have `Async` suffix
- Async event handlers must include `CancellationToken cancellationToken = default` as last parameter
- Domain models use OOP with business logic encapsulated
- Uses Ulid for identifiers (not Guid)
- Project settings: `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`

### Namespaces
Extension class should have namespace of extended object - e.g. IServiceCollection 

## Git Workflow
- Main branch: `main`
- Development branch: `develop`
- Create feature branches from `develop`