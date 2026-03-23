using chronos.time_logs.core.Communication.Sync;
using chronos.time_logs.core.Communication.Sync.Http;
using chronos.time_logs.core.Communication.Sync.Http.Clients;
using chronos.time_logs.core.Communication.Sync.Http.Configurations.Options;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class CommunicationHttpServicesExtensions
{
    internal static IServiceCollection AddHttpCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions(configuration);
            
        var options = services.GetOptions<HttpCommunicationOptions>();

        if (options.Enabled)
        {
            services.AddClients();
        }
        
        return services;
    }

    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.Configure<HttpCommunicationOptions>(configuration.GetSection(nameof(HttpCommunicationOptions)));

    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        var options = services.GetOptions<HttpCommunicationOptions>();

        var employeesClientOptions = options.Clients.Single(x => x.Name == nameof(HttpEmployeesClient));

        if (employeesClientOptions.ResiliencePattern == "Linear")
        {
            services
                .AddHttpClient<IEmployeesClient, HttpEmployeesClient>(config =>
                {
                    config.BaseAddress = new Uri(employeesClientOptions.Uri);
                })
                .AddPolicyHandler(GetPolicy(
                    employeesClientOptions.ResiliencePattern,
                    employeesClientOptions.Attempts,
                    employeesClientOptions.TimeSpan));
        }

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetPolicy(
        string type,
        int attempts,
        TimeSpan timeSpan) => type switch
    {
        "Linear" => GetLinearRetryPolicy(attempts, timeSpan),
        _ => GetLinearRetryPolicy(attempts, timeSpan)
    };
    
    private static IAsyncPolicy<HttpResponseMessage> GetLinearRetryPolicy(
        int attempts,
        TimeSpan timeSpan)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg 
                => msg.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            .WaitAndRetryAsync(
                retryCount: attempts,
                sleepDurationProvider: retryAttempt => timeSpan,
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Linear Retry {retryCount} after {timespan.TotalSeconds}s delay");
                });
    }
}