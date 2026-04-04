using chronos.contracts.core.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace chronos.contracts.core.Events.External;

public sealed record EmployeeDeleted(Ulid Id);

public interface IEmployeeDeletedEventHandler
{
    Task Handle(EmployeeDeleted @event, CancellationToken cancellationToken);
}

internal sealed class EmployeeDeletedEventHandler(
    IContractsRepository contractsRepository,
    IServiceScopeFactory serviceScopeFactory) : IEmployeeDeletedEventHandler
{
    /// <inheritdoc />
    public async Task Handle(EmployeeDeleted @event, CancellationToken cancellationToken)
    {
        var contracts = await contractsRepository.GetByEmployeeIdAsync(@event.Id, cancellationToken);

        var tasks = contracts.Select(contract => RemoveEmployeeFromContractAsync(
            contract.Id,
            @event.Id,
            cancellationToken));

        await Task.WhenAll(tasks);
    }

    private async Task RemoveEmployeeFromContractAsync(
        Domain.Identifiers.ContractId contractId,
        Ulid employeeId,
        CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IContractsRepository>();

        var contract = await repository.GetByIdAsync(contractId, cancellationToken);

        if (contract is null)
        {
            return;
        }

        contract.RemoveEmployeeByEmployeeId(employeeId);
        await repository.UpdateAsync(contract, cancellationToken);
    }
}
