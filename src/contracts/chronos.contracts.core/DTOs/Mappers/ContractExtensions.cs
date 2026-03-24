using chronos.contracts.core.DTOs.Responses;

namespace chronos.contracts.core.Domain;

/// <summary>
/// Extension methods for mapping <see cref="Contract"/> domain objects to DTOs.
/// </summary>
public static class ContractExtensions
{
    /// <summary>
    /// Maps a <see cref="Contract"/> aggregate to <see cref="ContractResponseDto"/>.
    /// </summary>
    /// <param name="contract">The contract to map.</param>
    /// <returns>The contract response DTO.</returns>
    public static ContractResponseDto ToDto(this Contract contract)
        => new(
            contract.Id.Value.ToString(),
            contract.CompanyDetails.Name,
            new ContractPeriodResponseDto(
                contract.ContractPeriod.AssignmentDate,
                contract.ContractPeriod.ClosingDate),
            contract.Employees.Select(MapEmployeeToDto).ToList());

    private static ContractEmployeeResponseDto MapEmployeeToDto(ContractEmployee employee)
        => new(
            employee.Id.Value.ToString(),
            employee.EmployeeId.ToString(),
            new AssignmentPeriodResponseDto(
                employee.AssignmentPeriod.From,
                employee.AssignmentPeriod.To),
            employee.AllocatedHours);
}
