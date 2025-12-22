using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static T GetOptions<T>(this IServiceCollection services) where T : class
    {
        var sp = services.BuildServiceProvider();
        return sp.GetRequiredService<IOptions<T>>().Value;
    }
}
