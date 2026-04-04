using chronos.employees.core.Services;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CoreServicesExtensions
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
            IConfiguration configuration)
    {
        services
            .AddSingleton(TimeProvider.System)
            .AddDal(configuration)
            .AddCommunication(configuration)
            .AddMemoryCache()
            .AddScoped<IEmployeeService, EmployeeService>()
            .Decorate<IEmployeeService, CachedEmployeeService>()
            .AddScoped<IEmployeeCache, CachedEmployeeService>()
            .AddBanner(configuration);

        return services;
    }
}