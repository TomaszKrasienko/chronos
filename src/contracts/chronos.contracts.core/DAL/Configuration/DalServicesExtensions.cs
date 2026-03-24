using chronos.contracts.core.DAL;
using chronos.contracts.core.DAL.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesExtensions
{
    internal static IServiceCollection AddDal(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddContext()
            .AddRepositories();

    private static IServiceCollection AddContext(
        this IServiceCollection services)
    {
        var mongoOptions = services.GetOptions<DalOptions>();
        services.AddDbContext<ContractsDbContext>(options => options.UseMongoDB(
            mongoOptions.ConnectionString,
            mongoOptions.DatabaseName));
        return services;
    }

    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DalOptions>(configuration.GetSection(nameof(DalOptions)));
        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IContractsRepository, ContractsRepository>();
        return services;
    }
}
