var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCore(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Health checks
app.MapGet("/health/live", () => Results.Ok())
    .WithName("LivenessCheck");

app.MapGet("/health/ready", () => Results.Ok())
    .WithName("ReadinessCheck");

// Time logs endpoints will be added here

app.Run();
