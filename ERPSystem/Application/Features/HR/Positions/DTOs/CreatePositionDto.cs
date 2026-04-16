namespace ERPSystem.Application.Features.HR.Positions.DTOs;

public class CreatePositionDto
{
    public string Name  { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    
}