using chronos.employees.core.DTOs.Requests;
using chronos.employees.core.DTOs.Responses;
using chronos.employees.core.Services;

const string getEmployeeByIdRouteName = "GetEmployeeById";

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
builder.Services.AddHealthChecks();
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

app.MapGet(
    "/api/employees",
    async (
        IEmployeeService employeeService,
        CancellationToken cancellationToken) =>
    {
        var employees = await employeeService.GetAllAsync(cancellationToken);

        var employeeDtos = employees.Select(e => new EmployeeDto(
            e.Id.Value.ToString(),
            e.FullName.FirstName,
            e.FullName.LastName,
            e.Email.Value,
            e.SupervisorId?.Value.ToString())).ToList();

        return Results.Ok(employeeDtos);
    })
    .WithName("GetAllEmployees")
    .WithOpenApi();

app.MapPost(
    "/api/employees",
    async (
        CreateEmployeeRequestDto request,
        HttpContext context,
        IEmployeeService employeeService,
        CancellationToken cancellationToken) =>
    {
        var result = await employeeService.CreateAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.SupervisorId,
            cancellationToken);

        context.AddResourceId(result.Id.Value);

        return Results.CreatedAtRoute(
            getEmployeeByIdRouteName,
            new { employeeId = result.Id.Value },
            null);
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
            employee.Id.Value.ToString(),
            employee.FullName.FirstName,
            employee.FullName.LastName,
            employee.Email.Value,
            employee.SupervisorId?.Value.ToString());

        return Results.Ok(employeeDto);
    })
    .WithName(getEmployeeByIdRouteName)
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
            e.Id.Value.ToString(),
            e.FullName.FirstName,
            e.FullName.LastName,
            e.Email.Value,
            e.SupervisorId?.Value.ToString())).ToList();

        return Results.Ok(subordinateDtos);
    })
    .WithName("GetSubordinates")
    .WithOpenApi();

app.MapDelete(
    "/api/employees/{employeeId}",
    async (
        Ulid employeeId,
        IEmployeeService employeeService,
        CancellationToken cancellationToken) =>
    {
        await employeeService.DeleteAsync(employeeId, cancellationToken);
        return Results.NoContent();
    })
    .WithName("DeleteEmployee")
    .WithOpenApi();

app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

app.Run();