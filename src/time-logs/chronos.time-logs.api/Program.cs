using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain;
using chronos.time_logs.core.DTOs.Requests;
using chronos.time_logs.core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithHeaders("X-Employee-Id");
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
    "/api/time-logs/years/{year:int}/months/{month:int}",
    async (
        int year,
        int month,
        HttpContext httpContext,
        IReadTimeReportService readTimeReportService,
        CancellationToken cancellationToken) =>
    {
        var employeeId = httpContext.GetEmployeeContext();
        if (employeeId is null)
        {
            return Results.Unauthorized();
        }

        var report = await readTimeReportService.GetByEmployeeAndPeriodAsync(
            new EmployeeId(employeeId.Value),
            month,
            year,
            cancellationToken);

        return report is null
            ? Results.NotFound()
            : Results.Ok(report.ToDto());
    })
    .WithName("GetTimeLogsByPeriod")
    .WithOpenApi();

app.MapPost(
    "/api/time-logs",
    async (
        CreateCurrentTimeLogRequestDto request,
        HttpContext httpContext,
        IWriteTimeReportService writeTimeReportService,
        CancellationToken cancellationToken) =>
    {
        var employeeId = httpContext.GetEmployeeContext();
        if (employeeId is null)
        {
            return Results.Unauthorized();
        }

        var timeLogId = await writeTimeReportService.AddTimeLogAsync(
            new EmployeeId(employeeId.Value),
            new ContractId(request.ContractId),
            request.Hours,
            request.Topic,
            request.Notes,
            cancellationToken);

        return Results.Ok(timeLogId.Value.ToString());
    })
    .WithName("CreateTimeLog")
    .WithOpenApi();

app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

app.Run();
