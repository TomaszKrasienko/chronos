using chronos.employees.api;
using chronos.employees.core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCore(builder.Configuration);

var app = builder.Build();
app.UseCors("AllowAll");
app.UseChronosExceptionHandling();
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapCore();

app.MapGet(
    "/api/employees",
    async (
        IEmployeeService employeeService,
        CancellationToken cancellationToken) =>
    {
        var employees = await employeeService.GetAllAsync(cancellationToken);

        var employeeDtos = employees.Select(e => new EmployeeDto(
            e.Id.ToString(),
            e.FirstName,
            e.LastName,
            e.Email,
            e.SupervisorId?.ToString())).ToList();

        return Results.Ok(employeeDtos);
    })
    .WithName("GetAllEmployees")
    .WithOpenApi();

app.MapPost(
    "/api/employees",
    async (
        CreateEmployeeRequestDto request,
        IEmployeeService employeeService,
        CancellationToken cancellationToken) =>
    {
        var result = await employeeService.CreateAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.SupervisorId,
            cancellationToken);

        return Results.Ok(result.Id.ToString());
    })
    .WithName("CreateEmployee")
    .WithOpenApi();

app.MapPatch(
    "/api/employees/{employeeId}/supervisors/{supervisorId}",
    async (
        Ulid employeeId,
        Ulid supervisorId,
        IEmployeeService employeeService,
        CancellationToken cancellationToken) =>
    {
        await employeeService.AssignSupervisorAsync(
            employeeId,
            supervisorId,
            cancellationToken);
        
        return Results.NoContent();
    })
    .WithName("AssignSupervisor")
    .WithOpenApi();

app.MapGet(
    "/api/employees/{employeeId}",
    async (
        Ulid employeeId,
        IEmployeeService employeeService,
        CancellationToken cancellationToken) =>
    {
        var employee = await employeeService.GetByIdAsync(
            employeeId,
            cancellationToken);

        if (employee is null)
        {
            return Results.NotFound();
        }

        var employeeDto = new EmployeeDto(
            employee.Id.ToString(),
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.SupervisorId?.ToString());

        return Results.Ok(employeeDto);
    })
    .WithName("RetrieveEmployee")
    .WithOpenApi();

app.MapGet(
    "/api/employees/{supervisorId}/subordinates",
    async (
        Ulid supervisorId,
        IEmployeeService employeeService,
        CancellationToken cancellationToken) =>
    {
        var subordinates = await employeeService.GetSubordinatesAsync(
            supervisorId,
            cancellationToken);

        var subordinateDtos = subordinates.Select(e => new EmployeeDto(
            e.Id.ToString(),
            e.FirstName,
            e.LastName,
            e.Email,
            e.SupervisorId?.ToString())).ToList();

        return Results.Ok(subordinateDtos);
    })
    .WithName("GetSubordinates")
    .WithOpenApi();

app.Run();