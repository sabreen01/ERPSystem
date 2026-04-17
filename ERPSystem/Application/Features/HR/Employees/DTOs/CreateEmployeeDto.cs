using ERPSystem.Domain.Enums;

namespace ERPSystem.Application.Features.HR.Employees.DTOs;

public record CreateEmployeeDto(
   
    string FirstName, 
    string LastName, 
    DateTime DateOfBirth, 
    Gender Gender,
    string NationalId, 
    string? PassportNo,
    string Nationality,
    string? PersonalEmail,
    string PersonalPhone,
    string? Address,

    DateTime HireDate, 
    ContractType ContractType,
    Guid BranchId, 
    Guid DepartmentId, 
    Guid PositionId, 

    decimal BasicSalary, 
    Guid SalaryStructureId,
    string? BankName,
    string? BankAccountNo,
    string? BankIban
);