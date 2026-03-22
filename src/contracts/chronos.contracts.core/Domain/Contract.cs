using chronos.contracts.core.Domain.Events;
using chronos.contracts.core.Domain.Identifiers;
using chronos.contracts.core.Domain.Rules;
using chronos.contracts.core.Domain.ValueObjects;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain;

/// <summary>
/// Aggregate root representing a contract with company details and assigned employees.
/// </summary>
public sealed class Contract : AggregateRoot<ContractId>
{
    private readonly List<ContractEmployee> _employees = [];

    /// <summary>
    /// Gets the company details associated with this contract.
    /// </summary>
    public CompanyDetails CompanyDetails { get; private set; }

    /// <summary>
    /// Gets the contract period including assignment and closing dates.
    /// </summary>
    public ContractPeriod ContractPeriod { get; private set; }

    /// <summary>
    /// Gets the list of employees assigned to this contract.
    /// </summary>
    public IReadOnlyList<ContractEmployee> Employees => _employees.AsReadOnly();

#pragma warning disable CS8618
    private Contract()
    {
    }
#pragma warning restore CS8618

    private Contract(
        ContractId id,
        CompanyDetails companyDetails,
        ContractPeriod contractPeriod) : base(id)
    {
        CompanyDetails = companyDetails;
        ContractPeriod = contractPeriod;
    }

    /// <summary>
    /// Creates a new contract with the specified company details and period.
    /// </summary>
    /// <param name="companyDetails">The company details.</param>
    /// <param name="contractPeriod">The contract period.</param>
    public static Contract Create(
        CompanyDetails companyDetails,
        ContractPeriod contractPeriod)
    {
        var contract = new Contract(
            ContractId.New(),
            companyDetails,
            contractPeriod);

        contract.AddDomainEvent(new ContractCreatedEvent(
            contract.Id,
            companyDetails.Name,
            contractPeriod.AssignmentDate,
            contractPeriod.ClosingDate));

        return contract;
    }

    /// <summary>
    /// Assigns an employee to this contract with specified period and hours.
    /// </summary>
    /// <param name="employeeId">The employee identifier from Employee bounded context.</param>
    /// <param name="assignmentPeriod">The period of assignment.</param>
    /// <param name="allocatedHours">The number of hours allocated.</param>
    public void AssignEmployee(
        Ulid employeeId,
        AssignmentPeriod assignmentPeriod,
        int allocatedHours)
    {
        CheckRule(new EmployeeAlreadyAssignedToContractRule(_employees, employeeId));

        var employee = ContractEmployee.Create(employeeId, assignmentPeriod, allocatedHours);
        _employees.Add(employee);

        AddDomainEvent(new EmployeeAssignedEvent(
            Id,
            employee.Id,
            employeeId,
            assignmentPeriod.From,
            assignmentPeriod.To,
            allocatedHours));
    }

    /// <summary>
    /// Removes an employee assignment from this contract. Operation is idempotent.
    /// </summary>
    /// <param name="contractEmployeeId">The contract employee identifier.</param>
    public void RemoveEmployee(ContractEmployeeId contractEmployeeId)
    {
        var employee = _employees.SingleOrDefault(e => e.Id == contractEmployeeId);
        if (employee is not null)
        {
            _employees.Remove(employee);

            AddDomainEvent(new EmployeeRemovedEvent(
                Id,
                contractEmployeeId,
                employee.EmployeeId));
        }
    }

    /// <summary>
    /// Updates the allocated hours for an employee assignment.
    /// </summary>
    /// <param name="contractEmployeeId">The contract employee identifier.</param>
    /// <param name="allocatedHours">The new number of allocated hours.</param>
    public void UpdateEmployeeAllocatedHours(ContractEmployeeId contractEmployeeId, int allocatedHours)
    {
        CheckRule(new EmployeeAssignmentMustExistRule(_employees, contractEmployeeId));

        var employee = _employees.Single(e => e.Id == contractEmployeeId);
        employee.UpdateAllocatedHours(allocatedHours);

        AddDomainEvent(new EmployeeHoursUpdatedEvent(
            Id,
            contractEmployeeId,
            employee.EmployeeId,
            allocatedHours));
    }

    /// <summary>
    /// Closes the contract with the specified closing date.
    /// </summary>
    /// <param name="closingDate">The closing date.</param>
    public void CloseContract(DateOnly closingDate)
    {
        ContractPeriod = ContractPeriod.Close(closingDate);

        AddDomainEvent(new ContractClosedEvent(Id, closingDate));
    }
}
