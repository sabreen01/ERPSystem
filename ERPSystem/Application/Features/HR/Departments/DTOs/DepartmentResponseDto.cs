namespace ERPSystem.Application.Features.HR.Departments.DTOs;


public record DepartmentResponseDto(
    Guid Id, 
    string Name, 
    string Code, 
    bool IsActive, 
    string? ParentName, 
    Guid? ParentId);