namespace chronos.time_logs.core.Communication.Sync.Http.DTOs;

/// <summary>
/// Response DTO representing a contract from the Contracts service.
/// </summary>
/// <param name="Id">The contract identifier.</param>
/// <param name="CompanyName">The company name.</param>
/// <param name="ContractPeriod">The contract period details.</param>
/// <param name="Employees">The list of assigned employees.</param>
internal sealed record ContractResponseDto(
    string Id,
    string CompanyName,
    ContractPeriodResponseDto ContractPeriod,
    IReadOnlyList<ContractEmployeeResponseDto> Employees);

/// <summary>
/// Response DTO representing a contract period.
/// </summary>
/// <param name="AssignmentDate">The contract assignment date.</param>
/// <param name="ClosingDate">The contract closing date, or null if active.</param>
internal sealed record ContractPeriodResponseDto(
    DateOnly AssignmentDate,
    DateOnly? ClosingDate);

/// <summary>
/// Response DTO representing an employee assigned to a contract.
/// </summary>
/// <param name="Id">The contract employee identifier.</param>
/// <param name="EmployeeId">The employee identifier from Employee bounded context.</param>
/// <param name="AssignmentPeriod">The assignment period details.</param>
/// <param name="AllocatedHours">The number of allocated hours.</param>
internal sealed record ContractEmployeeResponseDto(
    string Id,
    string EmployeeId,
    AssignmentPeriodResponseDto AssignmentPeriod,
    int AllocatedHours);

/// <summary>
/// Response DTO representing an assignment period.
/// </summary>
/// <param name="From">The assignment start date.</param>
/// <param name="To">The assignment end date, if defined.</param>
internal sealed record AssignmentPeriodResponseDto(
    DateOnly From,
    DateOnly? To);
