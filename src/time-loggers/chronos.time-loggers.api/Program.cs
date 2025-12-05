using chronos.time_loggers.api.Exceptions;
using chronos.time_loggers.core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandling();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCore(builder.Configuration);

var app = builder.Build();
app.UseExceptionHandler();
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapPost(
    "/api/time-loggers",
    async (
        CreateTimeLoggerRequestDto request,
        ITimeLoggerService timeLoggerService,
        CancellationToken cancellationToken) =>
    {
        var result = await timeLoggerService.CreateAsync(
            request.TimeSpan,
            request.TimeFrom,
            request.TimeTo,
            request.EmployeeId,
            request.Topic,
            request.Notes,
            cancellationToken);

        return Results.Ok(result.Id.ToString());
    })
    .WithName("CreateTimeLogger")
    .WithOpenApi();

app.Run();