using chronos.shared.configuration.Options;
using Figgle.Fonts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace chronos.shared.configuration.Services;

internal sealed class BannerService(
    ILogger<BannerService> logger,
    IOptions<AppOptions> options,
    InstanceOptions instanceOptions,
    IHostEnvironment environment) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine(FiggleFonts.Slant.Render(options.Value.Name));
        Console.WriteLine(FiggleFonts.Standard.Render($"Environment: {environment.EnvironmentName}"));
        
        logger.LogInformation($"Started instance with ID: {instanceOptions.Id}");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
