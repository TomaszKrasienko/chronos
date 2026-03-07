using chronos.notifications.core.Services;

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
builder.Services.AddHealthChecks();
builder.Services.AddCore(builder.Configuration);

var app = builder.Build();
app.UseCors("AllowAll");
app.UseChronosExceptionHandling();
app.MapOpenApi();

app.MapGet(
    "/api/notification-messages/unread",
    async (
        HttpContext context,
        INotificationMessagesService notificationMessagesService,
        CancellationToken cancellationToken) =>
    {
        var employeeId = context.GetEmployeeContext();

        if (employeeId is null)
        {
            return Results.Unauthorized();
        }

        var messages = await notificationMessagesService
            .GetUnreadAsync(
                employeeId.Value,
                cancellationToken);

        return Results.Ok(messages);
    })
    .WithName("GetUnreadNotificationsMessages")
    .WithOpenApi();

app.MapPut(
    "/api/notification-messages/{id}/read-status",
    async (
        Ulid id,
        HttpContext context,
        INotificationMessagesService notificationMessagesService,
        CancellationToken cancellationToken) =>
    {
        var employeeId = context.GetEmployeeContext();

        if (employeeId is null)
        {
            return Results.Unauthorized();
        }

        await notificationMessagesService
            .MarkAsReadAsync(
                id,
                employeeId.Value,
                cancellationToken);

        return Results.NoContent();
    })
    .WithName("MarkNotificationMessageAsRead")
    .WithOpenApi();

app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

app.UseHttpsRedirection();
app.Run();