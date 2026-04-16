namespace ERPSystem.Application.Features.HR.Positions.DTOs;

public class UpdatePositionDto
{
    
    public string Name { get; set; }
    public Guid DepartmentId { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public bool IsActive { get; set; }
}