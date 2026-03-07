using chronos.time_loggers.api;
using chronos.time_loggers.core.Services;

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
app.UseHttpsRedirection();

app.MapPost(
    "/api/time-loggers",
    async (
        HttpContext httpContext,
        CreateTimeLoggerRequestDto request,
        ITimeLoggerService timeLoggerService,
        CancellationToken cancellationToken) =>
    {
        var employeeId = httpContext.GetEmployeeContext();

        if (!employeeId.HasValue)
        {
            return Results.BadRequest("Employee context is required");
        }

        var result = await timeLoggerService.CreateAsync(
            request.TimeSpan,
            request.TimeFrom,
            request.TimeTo,
            employeeId.Value,
            request.Topic,
            request.Notes,
            cancellationToken);

        return Results.Ok(result.Id.ToString());
    })
    .WithName("CreateTimeLogger")
    .WithOpenApi();

app.MapGet(
    "/api/time-loggers/my",
    async (
        HttpContext httpContext,
        ITimeLoggerService timeLoggerService,
        CancellationToken cancellationToken = default) =>
    {
        var employeeId = httpContext.GetEmployeeContext();

        if (!employeeId.HasValue)
        {
            return Results.BadRequest("Employee context is required");
        }

        var timeLogs = await timeLoggerService.GetByEmployeeIdAsync(
            employeeId.Value,
            cancellationToken);

        var timeLogDtos = timeLogs.Select(tl => tl.ToDto()).ToList();

        return Results.Ok(timeLogDtos);
    })
    .WithName("GetMyTimeLogs")
    .WithOpenApi();

app.MapGet(
    "/api/time-loggers/subordinates",
    async (
        HttpContext httpContext,
        ITimeLoggerService timeLoggerService,
        CancellationToken cancellationToken = default) =>
    {
        var supervisorId = httpContext.GetEmployeeContext();

        if (!supervisorId.HasValue)
        {
            return Results.BadRequest("Employee context is required");
        }

        var groupedTimeLogs = await timeLoggerService.GetForSupervisorAsync(
            supervisorId.Value,
            cancellationToken);

        var employeeTimeLogsDtos = groupedTimeLogs.Select(g =>
            new EmployeeTimeLogsDto(
                g.EmployeeId.ToString(),
                g.FirstName,
                g.LastName,
                g.TimeLogs.Select(tl => tl.ToDto()).ToList()
            )).ToList();

        return Results.Ok(employeeTimeLogsDtos);
    })
    .WithName("GetSubordinatesTimeLogs")
    .WithOpenApi();

app.MapPatch(
    "/api/time-loggers/{id}/accept",
    async (
        HttpContext httpContext,
        Ulid id,
        ITimeLoggerService timeLoggerService,
        CancellationToken cancellationToken) =>
    {
        var supervisorId = httpContext.GetEmployeeContext();

        if (!supervisorId.HasValue)
        {
            return Results.BadRequest("Employee context is required");
        }

        await timeLoggerService.AcceptAsync(
            id,
            supervisorId.Value,
            cancellationToken);

        return Results.NoContent();
    })
    .WithName("AcceptTimeLog")
    .WithOpenApi();

app.MapPatch(
    "/api/time-loggers/{id}/reject",
    async (
        HttpContext httpContext,
        Ulid id,
        ITimeLoggerService timeLoggerService,
        CancellationToken cancellationToken) =>
    {
        var supervisorId = httpContext.GetEmployeeContext();

        if (!supervisorId.HasValue)
        {
            return Results.BadRequest("Employee context is required");
        }

        await timeLoggerService.RejectAsync(
            id,
            supervisorId.Value,
            cancellationToken);

        return Results.NoContent();
    })
    .WithName("RejectTimeLog")
    .WithOpenApi();

app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

app.Run();