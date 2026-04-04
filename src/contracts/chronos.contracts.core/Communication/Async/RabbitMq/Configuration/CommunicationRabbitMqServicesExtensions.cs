using chronos.contracts.core.DTOs.Responses;
using chronos.contracts.core.Events;
using chronos.contracts.core.Events.External;
using chronos.contracts.core.Services;
using chronos.shared.configuration.Options;
using chronos.shared.messaging.rabbit_mq.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class CommunicationRabbitMqServicesExtensions
{
    internal static IServiceCollection AddRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var appOptions = services.GetOptions<AppOptions>();

        services.AddChronosRabbitMq(configuration, appOptions.Name);

        services.AddConsumer<ContractCreated>(sp =>
        {
            return async (msg, ct, _) =>
            {
                using var scope = sp.CreateScope();
                var cacheService = scope.ServiceProvider.GetRequiredService<IContractsCache>();
                var contract = new ContractResponseDto(
                    msg.ContractId.ToString(),
                    msg.CompanyName,
                    new ContractPeriodResponseDto(msg.AssignmentDate, msg.ClosingDate),
                    []);
                await cacheService.CreateAsync(contract, ct);
            };
        });

        services.AddConsumer<EmployeeDeleted>(sp =>
        {
            return async (msg, ct, _) =>
            {
                using var scope = sp.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IEmployeeDeletedEventHandler>();
                await handler.Handle(msg, ct);
            };
        });

        return services;
    }
}
