using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace chronos.shared.configuration.Extensions;

public static class ServiceCollectionExtensions
{
    public static T GetOptions<T>(this IServiceCollection services) where T : class
    {
        var sp = services.BuildServiceProvider();
        return sp.GetRequiredService<IOptions<T>>().Value;
    }
}
