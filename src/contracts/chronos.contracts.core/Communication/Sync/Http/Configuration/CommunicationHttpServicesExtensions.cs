using chronos.contracts.core.Communication.Sync.Http;
using chronos.contracts.core.Communication.Sync.Http.Configuration;
using Microsoft.Extensions.Configuration;
using Refit;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class CommunicationHttpServicesExtensions
{
    internal static IServiceCollection AddHttpCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<HttpEmployeeOptions>(
            configuration.GetSection(nameof(HttpEmployeeOptions)));

        var options = services.GetOptions<HttpEmployeeOptions>();

        services
            .AddRefitClient<IEmployeeHttpClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(options.Url));

        services.AddScoped<IEmployeesClient, EmployeesClient>();

        return services;
    }
}
