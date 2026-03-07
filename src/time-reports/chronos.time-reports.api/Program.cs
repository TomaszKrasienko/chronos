using chronos.time_reports.api;
using chronos.time_reports.core.Services;

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


app.MapGet(
    "/api/time-reports",
    async (
        HttpContext httpContext,
        ITimeReportsService timeReportsService,
        CancellationToken cancellationToken = default) =>
    {
        var employeeId = httpContext.GetEmployeeContext();

        if (!employeeId.HasValue)
        {
            return Results.BadRequest("Employee context is required");
        }

        var timeReport = await timeReportsService.GetByEmployeeIdAsync(
            employeeId.Value,
            cancellationToken);

        if (timeReport is null)
        {
            return Results.NotFound();
        }

        var timeReportDto = timeReport.ToDto();

        return Results.Ok(timeReportDto);
    })
    .WithName("GetTimeReport")
    .WithOpenApi();

app.MapPost(
    "/api/time-reports/files/{employeeId}",
    async (
        string employeeId,
        ITimeReportsService timeReportsService,
        CancellationToken cancellationToken) =>
    {
        if (!Ulid.TryParse(employeeId, out var parsedEmployeeId))
        {
            return Results.BadRequest("Invalid employee ID format");
        }

        await timeReportsService.SaveToFileAsync(parsedEmployeeId, cancellationToken);
        return Results.Ok();
    })
    .WithName("SaveTimeReportToFile")
    .WithOpenApi();

app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

app.Run();