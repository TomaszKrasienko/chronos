using chronos.employees.core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCore(builder.Configuration);

var app = builder.Build();
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapCore();

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
            employee.SupervisorId?.ToString());

        return Results.Ok(employeeDto);
    })
    .WithName("RetrieveEmployee")
    .WithOpenApi();

app.Run();