using chronos.contracts.core.Domain.Identifiers;

namespace chronos.contracts.core.Services;

/// <summary>
/// Service for writing contract data.
/// </summary>
public interface IWriteContractsService
{
    /// <summary>
    /// Creates a new contract with company details.
    /// </summary>
    /// <param name="companyName">The name of the company.</param>
    /// <param name="assignmentDate">The contract assignment date.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created contract identifier.</returns>
    Task<ContractId> CreateContractAsync(
        string companyName,
        DateOnly assignmentDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Assigns an employee to a contract.
    /// </summary>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="from">The assignment start date.</param>
    /// <param name="to">The assignment end date.</param>
    /// <param name="allocatedHours">The number of allocated hours.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AssignEmployeeAsync(
        ContractId contractId,
        Ulid employeeId,
        DateOnly from,
        DateOnly? to,
        int allocatedHours,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an employee from a contract.
    /// </summary>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="contractEmployeeId">The contract employee identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task RemoveEmployeeAsync(
        ContractId contractId,
        ContractEmployeeId contractEmployeeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates allocated hours for an employee.
    /// </summary>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="contractEmployeeId">The contract employee identifier.</param>
    /// <param name="allocatedHours">The new number of allocated hours.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UpdateEmployeeAllocatedHoursAsync(
        ContractId contractId,
        ContractEmployeeId contractEmployeeId,
        int allocatedHours,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes a contract.
    /// </summary>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="closingDate">The closing date.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task CloseContractAsync(
        ContractId contractId,
        DateOnly closingDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an employee's assignment period.
    /// </summary>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="contractEmployeeId">The contract employee identifier.</param>
    /// <param name="from">The assignment start date.</param>
    /// <param name="to">The assignment end date.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UpdateEmployeeAssignmentPeriodAsync(
        ContractId contractId,
        ContractEmployeeId contractEmployeeId,
        DateOnly from,
        DateOnly? to,
        CancellationToken cancellationToken = default);
}
