# Chronos - Port Configuration

## Port Mapping

All services are configured with consistent port numbers for easy development:

### HTTP Ports
| Service | HTTP Port | HTTPS Port | URL |
|---------|-----------|------------|-----|
| **Blazor UI** | 5000 | 7000 | http://localhost:5000 |
| **Employees API** | 5001 | 7001 | http://localhost:5001/swagger |
| **Time Loggers API** | 5002 | 7002 | http://localhost:5002/swagger |
| **Time Reports API** | 5003 | 7003 | http://localhost:5003/swagger |
| **Notifications API** | 5004 | 7004 | http://localhost:5004/swagger |

## Running Services

### Quick Start - All Services

Start all services at once:
```bash
./run-all.sh
```

Stop all services:
```bash
./stop-all.sh
```

### Individual Services

#### Employees API
```bash
cd src/employees/chronos.employees.api
dotnet run --launch-profile http
```

#### Time Loggers API
```bash
cd src/time-loggers/chronos.time-loggers.api
dotnet run --launch-profile http
```

#### Time Reports API
```bash
cd src/time-reports/chronos.time-reports.api
dotnet run --launch-profile http
```

#### Notifications API
```bash
cd src/notifications/chronos.notifications.api
dotnet run --launch-profile http
```

#### Blazor UI
```bash
cd src/ui/chronos.ui
dotnet run --launch-profile http
```

## Launch Profiles

Each project has two launch profiles configured in `Properties/launchSettings.json`:

- **http** - HTTP only (ports 5000-5004)
- **https** - HTTPS with HTTP fallback (ports 7000-7004 + 5000-5004)

### Using HTTPS
```bash
dotnet run --launch-profile https
```

## Features

- **Auto-open browser** - All services automatically open in browser on start
- **Swagger UI** - All APIs open Swagger documentation by default
- **Consistent ports** - Easy to remember port scheme (5000-5004)
- **CORS enabled** - APIs configured to accept requests from UI

## Port Conflicts

If you encounter port conflicts, you can:

1. **Check what's using the port:**
   ```bash
   lsof -i :5000
   ```

2. **Kill the process:**
   ```bash
   kill -9 <PID>
   ```

3. **Or change ports** in `launchSettings.json`

## Development Workflow

### Recommended startup order:

1. Start backend APIs first (they have mock data)
2. Start UI last (it will connect to APIs or use mock data)

Or simply use `./run-all.sh` which handles the correct order automatically!

## Notes

- The UI will gracefully fall back to mock data if APIs are unavailable
- All services use Development environment by default
- Swagger is automatically enabled for all APIs in Development mode
