using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Domain.Entities.HR;
[Table(name : "Position" , Schema = "HR") ]
public class Position : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; 
    public bool IsActive { get; set; } = true;

    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }

    public Guid DepartmentId { get; set; }
    public virtual Department Department { get; set; } = null!;
}