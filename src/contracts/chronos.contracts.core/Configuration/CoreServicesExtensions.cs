using chronos.contracts.core.Services;
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
            .AddServices();

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IWriteContractsService, ContractsService>();
        services.AddScoped<IReadContractsService, ContractsService>();
        return services;
    }
}
