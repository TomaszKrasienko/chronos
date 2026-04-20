
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

app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

app.UseHttpsRedirection();
app.Run();