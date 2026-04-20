using chronos.contracts.core.DAL;
using chronos.shared.kernel.Identifiers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace chronos.contracts.core.Events.External;

public sealed record EmployeeDeleted(Ulid Id);

public interface IEmployeeDeletedEventHandler
{
    Task Handle(EmployeeDeleted @event, CancellationToken cancellationToken);
}

internal sealed class EmployeeDeletedEventHandler(
    ILogger<EmployeeDeletedEventHandler> logger,
    IContractsRepository contractsRepository,
    IServiceScopeFactory serviceScopeFactory) : IEmployeeDeletedEventHandler
{
    /// <inheritdoc />
    public async Task Handle(EmployeeDeleted @event, CancellationToken cancellationToken)
    {
        var employeeId = new EmployeeId(@event.Id);
        var contracts = await contractsRepository.GetByEmployeeIdAsync(employeeId, cancellationToken);
        
        logger.LogInformation("Removing employee with ID {@EmployeeId} from {@ContractsCount} contracts.", @event.Id, contracts.Count);

        var tasks = contracts.Select(contract => RemoveEmployeeFromContractAsync(
            contract.Id,
            employeeId,
            cancellationToken));

        await Task.WhenAll(tasks);
    }

    private async Task RemoveEmployeeFromContractAsync(
        ContractId contractId,
        EmployeeId employeeId,
        CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IContractsRepository>();

        var contract = await repository.GetByIdAsync(contractId, cancellationToken);

        if (contract is null)
        {
            return;
        }

        contract.RemoveEmployee(employeeId);
        await repository.UpdateAsync(contract, cancellationToken);
    }
}
