using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using chronos.ui;
using chronos.ui.Models;
using chronos.ui.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load API settings from configuration
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>() ?? new ApiSettings();
var endpoints = apiSettings.GetCurrentEndpoints();

// Configure HttpClients for different APIs
builder.Services.AddHttpClient("EmployeesAPI", client =>
{
    client.BaseAddress = new Uri(endpoints.EmployeesApi);
});

builder.Services.AddHttpClient("TimeLoggersAPI", client =>
{
    client.BaseAddress = new Uri(endpoints.TimeLoggersApi);
});

builder.Services.AddHttpClient("TimeReportsAPI", client =>
{
    client.BaseAddress = new Uri(endpoints.TimeReportsApi);
});

builder.Services.AddHttpClient("NotificationsAPI", client =>
{
    client.BaseAddress = new Uri(endpoints.NotificationsApi);
});

// Register services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ITimeLogService, TimeLogService>();
builder.Services.AddScoped<ITimeReportService, TimeReportService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<AppStateService>();

// TODO: Add SignalR when ready
// builder.Services.AddSingleton<HubConnection>(sp =>
// {
//     return new HubConnectionBuilder()
//         .WithUrl("http://localhost:5004/notificationHub")
//         .Build();
// });

await builder.Build().RunAsync();
