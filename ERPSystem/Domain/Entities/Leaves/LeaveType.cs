using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Domain.Entities.Leaves;

[Table(name: "LeaveType", Schema = "Leaves")]
public class LeaveType : BaseEntity
{
    public string Name { get; set; } = string.Empty;         
    public string Code { get; set; } = string.Empty;         
    public decimal DaysPerYear { get; set; }                  
    public bool IsPaid { get; set; } = true;                  
    public bool CarryForward { get; set; } = false;           
    public int? MaxCarryDays { get; set; }                     
    public bool RequiresApproval { get; set; } = true;         
    public int MinNoticeDays { get; set; } = 0;                
    public bool IsActive { get; set; } = true;                 
}
