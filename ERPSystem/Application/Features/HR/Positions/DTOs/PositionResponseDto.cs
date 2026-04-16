namespace ERPSystem.Application.Features.HR.Positions.DTOs;

public record PositionResponseDto(
    Guid Id,
    string Name,
    string Code,
    decimal MinSalary,
    decimal MaxSalary,
    string DepartmentName, 
    bool IsActive
);
