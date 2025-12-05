using chronos.shared.configuration.Options;
using Figgle.Fonts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace chronos.shared.configuration.Services;

internal sealed class BannerService(
    IOptions<AppOptions> options,
    IHostEnvironment environment) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine(FiggleFonts.Slant.Render(options.Value.Name));
        Console.WriteLine(FiggleFonts.Standard.Render($"Environment: {environment.EnvironmentName}"));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
