using chronos.contracts.core.Communication.Sync.Http;
using chronos.contracts.core.DAL;
using chronos.contracts.core.Domain;
using chronos.contracts.core.Domain.Events;
using chronos.contracts.core.Domain.Identifiers;
using chronos.contracts.core.Domain.ValueObjects;
using chronos.contracts.core.DTOs.Responses;
using chronos.shared.kernel.Exceptions;
using chronos.shared.messaging;

namespace chronos.contracts.core.Services;

/// <summary>
/// Service for managing contracts.
/// </summary>
internal sealed class ContractsService(
    IContractsRepository contractsRepository,
    IMessageDispatcher messageDispatcher,
    IEmployeesClient employeesClient)
    : IWriteContractsService, IReadContractsService
{
    public async Task<ContractId> CreateContractAsync(
        string companyName,
        DateOnly assignmentDate,
        CancellationToken cancellationToken = default)
    {
        if (await contractsRepository.ExistsByCompanyNameAsync(companyName, cancellationToken))
        {
            throw new NotUniqueException(
                nameof(Contract),
                [companyName]);
        }

        var companyDetails = CompanyDetails.Create(companyName);
        var contractPeriod = ContractPeriod.Create(assignmentDate);
        var contract = Contract.Create(companyDetails, contractPeriod);

        await contractsRepository.AddAsync(contract, cancellationToken);

        var domainEvent = contract.DomainEvents.OfType<ContractCreatedEvent>().Single();
        await messageDispatcher.Send(domainEvent.ToIntegrationEvent(), cancellationToken);
        contract.ClearDomainEvents();

        return contract.Id;
    }

    public async Task AssignEmployeeAsync(
        ContractId contractId,
        Ulid employeeId,
        DateOnly from,
        DateOnly? to,
        int allocatedHours,
        CancellationToken cancellationToken = default)
    {
        if (!await employeesClient.DoesEmployeeExistAsync(employeeId, cancellationToken))
        {
            throw new NotFoundException("Employee", [employeeId.ToString()]);
        }

        var contract = await GetContractAsync(contractId, cancellationToken);
        var assignmentPeriod = AssignmentPeriod.Create(from, to);

        contract.AssignEmployee(employeeId, assignmentPeriod, allocatedHours);

        await contractsRepository.UpdateAsync(contract, cancellationToken);
    }

    public async Task RemoveEmployeeAsync(
        ContractId contractId,
        ContractEmployeeId contractEmployeeId,
        CancellationToken cancellationToken = default)
    {
        var contract = await GetContractAsync(contractId, cancellationToken);

        contract.RemoveEmployee(contractEmployeeId);

        await contractsRepository.UpdateAsync(contract, cancellationToken);
    }

    public async Task UpdateEmployeeAllocatedHoursAsync(
        ContractId contractId,
        ContractEmployeeId contractEmployeeId,
        int allocatedHours,
        CancellationToken cancellationToken = default)
    {
        var contract = await GetContractAsync(contractId, cancellationToken);

        contract.UpdateEmployeeAllocatedHours(contractEmployeeId, allocatedHours);

        await contractsRepository.UpdateAsync(contract, cancellationToken);
    }

    public async Task CloseContractAsync(
        ContractId contractId,
        DateOnly closingDate,
        CancellationToken cancellationToken = default)
    {
        var contract = await GetContractAsync(contractId, cancellationToken);

        contract.CloseContract(closingDate);

        await contractsRepository.UpdateAsync(contract, cancellationToken);
    }

    public async Task UpdateEmployeeAssignmentPeriodAsync(
        ContractId contractId,
        ContractEmployeeId contractEmployeeId,
        DateOnly from,
        DateOnly? to,
        CancellationToken cancellationToken = default)
    {
        var contract = await GetContractAsync(contractId, cancellationToken);
        var assignmentPeriod = AssignmentPeriod.Create(from, to);

        contract.UpdateEmployeeAssignmentPeriod(contractEmployeeId, assignmentPeriod);

        await contractsRepository.UpdateAsync(contract, cancellationToken);
    }

    public async Task<ContractResponseDto?> GetByIdAsync(
        ContractId contractId,
        CancellationToken cancellationToken = default)
    {
        var contract = await contractsRepository.GetByIdAsync(contractId, cancellationToken);
        return contract?.ToDto();
    }

    public async Task<IReadOnlyList<ContractResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var contracts = await contractsRepository.GetAllAsync(cancellationToken);
        return contracts.Select(c => c.ToDto()).ToList();
    }

    private async Task<Contract> GetContractAsync(
        ContractId contractId,
        CancellationToken cancellationToken)
    {
        var contract = await contractsRepository.GetByIdAsync(contractId, cancellationToken);

        return contract ?? throw new NotFoundException(nameof(Contract), [contractId.Value.ToString()]);
    }
}
