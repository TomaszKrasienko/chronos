namespace chronos.contracts.core.DTOs.Responses;

/// <summary>
/// Response DTO representing a contract.
/// </summary>
/// <param name="Id">The contract identifier.</param>
/// <param name="CompanyName">The company name.</param>
/// <param name="ContractPeriod">The contract period details.</param>
/// <param name="Employees">The list of assigned employees.</param>
public sealed record ContractResponseDto(
    string Id,
    string CompanyName,
    ContractPeriodResponseDto ContractPeriod,
    IReadOnlyList<ContractEmployeeResponseDto> Employees);

/// <summary>
/// Response DTO representing a contract period.
/// </summary>
/// <param name="AssignmentDate">The contract assignment date.</param>
/// <param name="ClosingDate">The contract closing date, or null if active.</param>
public sealed record ContractPeriodResponseDto(
    DateOnly AssignmentDate,
    DateOnly? ClosingDate);
