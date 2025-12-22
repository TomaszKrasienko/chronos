# Chronos UI - Time Tracking System

Blazor WebAssembly application for managing employees, time logs, and time reports.

## Features

- **Employee Management** - View, add, and manage employees with supervisor assignment
- **Time Logs** - Track work hours with topic, notes, and approval workflow
  - **Employee Selector** - Choose which employee you're acting as
  - Auto-filters time logs for selected employee in Employee view
- **Time Reports** - View monthly reports with statistics
  - **Employee Selector** - View reports for specific employee
  - Filtered by selected employee context
- **Notifications** - Mock notification system (ready for SignalR integration)
  - **Employee Selector** - Context-aware notifications
- **Context Switcher** - Switch between Employee, Supervisor, and Admin views
- **Employee Context** - Select employee identity for time logging and reporting

## Architecture

The application integrates with three separate .NET Web APIs:

- **Employees API** (http://localhost:5001) - Employee management
- **Time Loggers API** (http://localhost:5002) - Time logging
- **Time Reports API** (http://localhost:5003) - Reporting

## Prerequisites

- .NET 9.0 SDK or higher
- A modern web browser (Chrome, Firefox, Edge)

## Project Structure

```
chronos.ui/
├── Components/          # Reusable Blazor components
│   └── ContextSwitcher.razor
├── Layout/             # Layout components
│   ├── MainLayout.razor
│   └── NavMenu.razor
├── Models/             # Data models
│   ├── Employee.cs
│   ├── TimeLog.cs
│   ├── TimeReport.cs
│   ├── AppContext.cs
│   └── Notification.cs
├── Pages/              # Page components
│   ├── Employees.razor
│   ├── TimeLogs.razor
│   ├── TimeReports.razor
│   └── Notifications.razor
├── Services/           # HTTP services
│   ├── IEmployeeService.cs
│   ├── EmployeeService.cs
│   ├── ITimeLogService.cs
│   ├── TimeLogService.cs
│   ├── ITimeReportService.cs
│   ├── TimeReportService.cs
│   └── AppStateService.cs
└── wwwroot/            # Static files
```

## Running the Application

### Option 1: Standalone (with mock data)

The application includes mock data and will work even if the APIs are not running.

```bash
cd src/ui/chronos.ui
dotnet run --launch-profile http
```

Navigate to http://localhost:5000

### Option 2: With Backend APIs

From the project root, use the convenience script:
```bash
./run-all.sh
```

Or start services individually:

1. **Start the Employees API**:
   ```bash
   cd src/employees/chronos.employees.api
   dotnet run --launch-profile http
   ```

2. **Start the Time Loggers API**:
   ```bash
   cd src/time-loggers/chronos.time-loggers.api
   dotnet run --launch-profile http
   ```

3. **Start the Time Reports API**:
   ```bash
   cd src/time-reports/chronos.time-reports.api
   dotnet run --launch-profile http
   ```

4. **Start the Notifications API**:
   ```bash
   cd src/notifications/chronos.notifications.api
   dotnet run --launch-profile http
   ```

5. **Start the UI**:
   ```bash
   cd src/ui/chronos.ui
   dotnet run --launch-profile http
   ```

**Stop all services:**
```bash
./stop-all.sh
```

## Configuration

### API Endpoints

API endpoints are configured in `Program.cs`:

```csharp
builder.Services.AddHttpClient("EmployeesAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001");
});

builder.Services.AddHttpClient("TimeLoggersAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5002");
});

builder.Services.AddHttpClient("TimeReportsAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5003");
});
```

To change the API endpoints, modify these URLs.

## Employee Context Selector

On Time Logs, Time Reports, and Notifications pages, you'll see an **Employee Selector** component:

- **Select Employee** - Choose which employee context you're acting as
- **Current Context Display** - Shows selected employee and their supervisor
- **Status Badge** - Visual confirmation of selected employee

### Why Employee Selector?

The UI needs to know which employee you're acting as for:
- **Time Logs** - Creates time logs with the selected employee's ID (sent via X-Employee-Id header)
- **Time Reports** - Filters reports to show only selected employee's data
- **Notifications** - Context-aware notifications for the selected employee

### How it Works

1. **Select an employee** from the dropdown
2. The selection is saved in `AppStateService.CurrentEmployeeId`
3. Time log creation sends `X-Employee-Id` header to the API
4. Reports and logs are automatically filtered for the selected employee

## Features by Context

### Employee View
- Add time logs for selected employee
- View time reports for selected employee
- View employee list (read-only)
- **Must select employee** before adding time logs

### Supervisor View
- All Employee permissions
- Approve/reject time logs from subordinates
- View team reports
- Can switch between different employee contexts

### Admin View
- All Supervisor permissions
- Add/edit employees
- Assign supervisors
- Full access to all features
- Can act as any employee

## Mock Data

The application includes mock data for development and testing:

- **Employees**: 3 sample employees (John Doe, Jane Smith, Bob Johnson)
- **Time Logs**: 3 sample time entries with different statuses
- **Time Reports**: 3 sample monthly reports
- **Notifications**: 5 sample notifications

Mock data is returned automatically when APIs are unavailable.

## TODO / Future Enhancements

- [ ] **SignalR Integration** - Replace mock notifications with real-time SignalR hub
- [ ] **Authentication** - Add user authentication and authorization
- [ ] **API Error Handling** - Improved error messages and retry logic
- [ ] **Pagination** - Add pagination for large data sets
- [ ] **Search/Filter** - Enhanced search and filtering capabilities
- [ ] **Date Range Selection** - Filter time logs and reports by date range
- [ ] **Export** - Export reports to PDF/Excel
- [ ] **Dark Mode** - Theme switcher
- [ ] **Localization** - Multi-language support

## Technologies Used

- **Blazor WebAssembly** - Frontend framework
- **Bootstrap 5** - UI framework
- **Bootstrap Icons** - Icon library
- **HttpClient** - API communication
- **Data Annotations** - Form validation

## Troubleshooting

### CORS Errors

If you see CORS errors in the browser console, ensure the backend APIs have CORS configured:

```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ...
app.UseCors();
```

### API Connection Issues

The application will gracefully fall back to mock data if APIs are unavailable. Check the browser console for connection errors.

### Build Errors

Ensure you have .NET 9.0 SDK installed:
```bash
dotnet --version
```

Clean and rebuild:
```bash
dotnet clean
dotnet build
```

## License

This project is part of the Chronos time tracking system.
