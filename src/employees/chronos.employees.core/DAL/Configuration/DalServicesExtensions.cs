using chronos.employees.core.DAL.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesExtensions
{
    internal static IServiceCollection AddDal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DalOptions>(configuration.GetSection(nameof(DalOptions)));
        return services;
    }
}