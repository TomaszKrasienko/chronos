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

app.UseHttpsRedirection();
app.Run();