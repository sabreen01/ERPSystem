using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Domain.Entities.HR;
[Table(name : "Department" , Schema = "HR") ]
public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; 
    public bool IsActive { get; set; } = true;

    public Guid? ParentDepartmentId { get; set; }
    public virtual Department? ParentDepartment { get; set; }
    
    public virtual ICollection<Department> ChildDepartments { get; set; } = new List<Department>();

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}