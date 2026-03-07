# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Chronos is a microservices-based time tracking system built with .NET 9 and Blazor WebAssembly. Employees submit time-logs that supervisors approve/reject. Time reports aggregate and summarize work hours.

## Skills
Skills to load to context are in .claude/skills

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

**Infrastructure ports:**
- MongoDB: 10011
- RabbitMQ: 10012 (AMQP), 10013 (Management UI)

**Service ports:**
- UI: 5000 | Employees: 5001 | Time Loggers: 5002 | Time Reports: 5003 | Notifications: 5004

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

## Code Conventions

- Async methods must have `Async` suffix
- Async event handlers must include `CancellationToken cancellationToken = default` as last parameter
- Domain models use OOP with business logic encapsulated
- Uses Ulid for identifiers (not Guid)
- Project settings: `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`

## Technologies

- .NET 9, MongoDB with EF Core, RabbitMQ, gRPC (inter-service sync communication)
- Scrutor for decorator pattern in DI
- Swashbuckle for Swagger UI
- YARP for reverse proxy

## Git Workflow

- Main branch: `main`
- Development branch: `develop`
- Create feature branches from `develop`
