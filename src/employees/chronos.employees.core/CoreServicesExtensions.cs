using chronos.employees.core.DAL.Configuration;
using chronos.employees.core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
public static class CoreServicesExtensions
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
            IConfiguration configuration)
        => services
            .AddDal(configuration)
            .AddScoped<IEmployeeService, EmployeeService>();
}