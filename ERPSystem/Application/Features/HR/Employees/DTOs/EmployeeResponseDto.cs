namespace ERPSystem.Application.Features.HR.Employees.DTOs;

public record EmployeeResponseDto(
    Guid Id,
    string EmployeeNo,
    string FirstName,
    string LastName,
    string? PersonalEmail,
    string? PositionName,
    string? DepartmentName,
    string? BranchName,
    bool IsActive
);
