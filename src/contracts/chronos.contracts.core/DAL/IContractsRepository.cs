using chronos.contracts.core.Domain;
using chronos.contracts.core.Domain.Identifiers;

namespace chronos.contracts.core.DAL;

/// <summary>
/// Repository for managing contracts persistence.
/// </summary>
public interface IContractsRepository
{
    /// <summary>
    /// Gets a contract by its identifier.
    /// </summary>
    /// <param name="id">The contract identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The contract if found; otherwise, null.</returns>
    Task<Contract?> GetByIdAsync(ContractId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all contracts.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>List of all contracts.</returns>
    Task<IReadOnlyList<Contract>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a contract with the specified company name exists.
    /// </summary>
    /// <param name="companyName">The company name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    Task<bool> ExistsByCompanyNameAsync(string companyName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new contract.
    /// </summary>
    /// <param name="contract">The contract to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AddAsync(Contract contract, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing contract.
    /// </summary>
    /// <param name="contract">The contract to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UpdateAsync(Contract contract, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all contracts that have the specified employee assigned.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>List of contracts with the employee assigned.</returns>
    Task<IReadOnlyList<Contract>> GetByEmployeeIdAsync(Ulid employeeId, CancellationToken cancellationToken = default);
}
