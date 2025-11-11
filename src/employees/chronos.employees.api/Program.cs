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
app.UseHttpsRedirection();

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

app.Run();