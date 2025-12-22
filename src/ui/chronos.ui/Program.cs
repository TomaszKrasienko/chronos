using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using chronos.ui;
using chronos.ui.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClients for different APIs
builder.Services.AddHttpClient("EmployeesAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001");
});

builder.Services.AddHttpClient("TimeLoggersAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5002");
});

builder.Services.AddHttpClient("TimeReportsAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5003");
});

builder.Services.AddHttpClient("NotificationsAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5004");
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
