namespace ERPSystem.Application.Features.HR.Departments.DTOs;

public record UpdateDepartmentDto(
    string Name,
    string Code, 
    Guid? ParentId, 
    bool IsActive);