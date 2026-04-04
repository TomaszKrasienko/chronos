using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace chronos.shared.messaging.outbox.DAL;

internal sealed class OutboxMigrator(
    ILogger<OutboxMigrator> logger,
    IServiceProvider sp) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = sp.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OutboxDbContext>();
        
        logger.LogInformation("Starting outbox migrator");
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}