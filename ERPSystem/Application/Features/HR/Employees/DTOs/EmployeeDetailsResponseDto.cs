using ERPSystem.Domain.Enums;

namespace ERPSystem.Application.Features.HR.Employees.DTOs;

public record EmployeeDetailsResponseDto(
    Guid Id,
    string EmployeeNo,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    Gender Gender,
    string NationalId,
    string Nationality,
    string? PersonalEmail,
    string PersonalPhone,
    DateTime HireDate,
    ContractType ContractType,
    Guid PositionId,
    string? PositionName,
    Guid DepartmentId,
    string? DepartmentName,
    Guid BranchId,
    string? BranchName,
    decimal BasicSalary,
    bool IsActive
);
