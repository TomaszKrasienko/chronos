namespace chronos.contracts.core.DTOs.Responses;

/// <summary>
/// Response DTO representing an employee assigned to a contract.
/// </summary>
/// <param name="Id">The contract employee identifier.</param>
/// <param name="EmployeeId">The employee identifier from Employee bounded context.</param>
/// <param name="AssignmentPeriod">The assignment period details.</param>
/// <param name="AllocatedHours">The number of allocated hours.</param>
public sealed record ContractEmployeeResponseDto(
    string Id,
    string EmployeeId,
    AssignmentPeriodResponseDto AssignmentPeriod,
    int AllocatedHours);

/// <summary>
/// Response DTO representing an assignment period.
/// </summary>
/// <param name="From">The assignment start date.</param>
/// <param name="To">The assignment end date.</param>
public sealed record AssignmentPeriodResponseDto(
    DateOnly From,
    DateOnly To);
