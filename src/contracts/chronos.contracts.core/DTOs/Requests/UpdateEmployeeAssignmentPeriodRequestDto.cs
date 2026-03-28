namespace chronos.contracts.core.DTOs.Requests;

public sealed record UpdateEmployeeAssignmentPeriodRequestDto(
    DateOnly From,
    DateOnly? To);
