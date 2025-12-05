using chronos.employees.core.Communication.Configuration;
using chronos.employees.core.Services;
using chronos.shared.configuration.Extensions;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CoreServicesExtensions
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
            IConfiguration configuration)
        => services
            .AddDal(configuration)
            .AddCommunication(configuration)
            .AddScoped<IEmployeeService, EmployeeService>()
            .AddBanner(configuration);
}