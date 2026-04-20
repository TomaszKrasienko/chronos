using chronos.contracts.core.Domain;
using chronos.contracts.core.Domain.ValueObjects;
using chronos.shared.kernel.Identifiers;

namespace chronos.tests.shared.Factories;

/// <summary>
/// Factory for creating <see cref="Contract"/> instances in tests.
/// </summary>
public static class ContractFactory
{
    public static Contract Create(
        CompanyDetails? companyDetails = null,
        ContractPeriod? contractPeriod = null)
        => Contract.Create(
            companyDetails ?? CompanyDetailsFactory.Create(),
            contractPeriod ?? ContractPeriodFactory.Create());

    public static Contract CreateWithEmployee(
        EmployeeId? employeeId = null,
        AssignmentPeriod? assignmentPeriod = null,
        int allocatedHours = 160)
    {
        var contract = Create();
        contract.AssignEmployee(
            employeeId ?? EmployeeId.New(),
            assignmentPeriod ?? AssignmentPeriodFactory.Create(),
            allocatedHours);
        return contract;
    }
}
