# Chronos - Time Tracking System

A microservices-based time tracking system built with .NET 9 and Blazor WebAssembly.

## Quick Start

### Start all services at once:
```bash
./run-all.sh
```

### Stop all services:
```bash
./stop-all.sh
```

Then open http://localhost:5000 in your browser!

## Architecture

### Services & Ports

| Service | Port | Technology | Description |
|---------|------|------------|-------------|
| **Blazor UI** | 5000 | Blazor WebAssembly | Frontend application |
| **Employees API** | 5001 | ASP.NET Core Web API | Employee management |
| **Time Loggers API** | 5002 | ASP.NET Core Web API | Time logging |
| **Time Reports API** | 5003 | ASP.NET Core Web API | Reporting |
| **Notifications API** | 5004 | ASP.NET Core Web API | Notifications |

### Shared Libraries
- **chronos.shared.configuration** - Configuration utilities
- **chronos.shared.exceptions** - Exception handling & middleware
- **chronos.shared.identity-context** - Employee context from HTTP headers
- **chronos.shared.messaging** - Messaging abstractions
- **chronos.shared.messaging.rabbit-mq** - RabbitMQ implementation

## Project Structure

```
chronos/
├── src/
│   ├── employees/           # Employee management
│   │   ├── chronos.employees.api
│   │   └── chronos.employees.core
│   ├── time-loggers/        # Time logging
│   │   ├── chronos.time-loggers.api
│   │   └── chronos.time-loggers.core
│   ├── time-reports/        # Reporting
│   │   ├── chronos.time-reports.api
│   │   └── chronos.time-reports.core
│   ├── notifications/       # Notifications
│   │   ├── chronos.notifications.api
│   │   └── chronos.notifications.core
│   ├── ui/                  # Blazor WebAssembly UI
│   │   └── chronos.ui
│   └── shared/              # Shared libraries
├── run-all.sh               # Start all services
├── stop-all.sh              # Stop all services
└── PORTS.md                 # Port configuration details
```

## Features

### Blazor UI
- Employee Management - Add, view, and manage employees
- Time Logging - Track work hours with approval workflow
- Time Reports - Monthly statistics and summaries
- Notifications - System notifications (mock, ready for SignalR)
- Context Switcher - Switch between Employee/Supervisor/Admin views
- Mock data fallback when APIs unavailable

### Backend APIs
- RESTful APIs with Swagger documentation
- MongoDB for data persistence
- RabbitMQ for async messaging
- Exception handling with Problem Details (RFC 7807)
- CORS enabled for local development

## Technologies

- **.NET 9** - Latest .NET framework
- **Blazor WebAssembly** - Frontend SPA framework
- **ASP.NET Core** - Backend APIs
- **MongoDB** - Document database
- **RabbitMQ** - Message broker
- **Bootstrap 5** - UI framework
- **Bootstrap Icons** - Icon library

## Development

### Prerequisites
- .NET 9.0 SDK
- MongoDB (for data persistence)
- RabbitMQ (for messaging)

### Build
```bash
dotnet build
```

### Run Individual Service
```bash
cd src/employees/chronos.employees.api
dotnet run --launch-profile http
```

See [PORTS.md](PORTS.md) for detailed port configuration and running instructions.

## Documentation

- [Port Configuration](PORTS.md) - All service ports and launch profiles
- [UI Documentation](src/ui/chronos.ui/README.md) - Blazor UI details

## Contributing

This is a learning/demo project showcasing:
- Microservices architecture
- Event-driven design with RabbitMQ
- CQRS patterns
- Clean Architecture
- Blazor WebAssembly
- Problem Details error handling 