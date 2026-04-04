using chronos.contracts.core.Events.External;
using chronos.contracts.core.Services;
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
            .AddDal(configuration)
            .AddCommunication(configuration)
            .AddMemoryCache()
            .AddScoped<IWriteContractsService, ContractsService>()
            .AddScoped<IReadContractsService, ContractsService>()
            .Decorate<IReadContractsService, CachedContractsService>()
            .AddScoped<IContractsCache, CachedContractsService>()
            .AddScoped<IEmployeeDeletedEventHandler, EmployeeDeletedEventHandler>();

        return services;
    }
}
