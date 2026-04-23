using System.ComponentModel.DataAnnotations.Schema;
using ERPSystem.Domain.Entities.HR;

namespace ERPSystem.Domain.Entities.Leaves;

[Table(name: "LeaveBalance", Schema = "Leaves")]
public class LeaveBalance : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public Guid LeaveTypeId { get; set; }
    public LeaveType? LeaveType { get; set; }

    public int FiscalYear { get; set; }                   
    public decimal OpeningBalance { get; set; }           
    public decimal UsedDays { get; set; } = 0;            
    public decimal CarriedForward { get; set; } = 0;      
    public decimal Adjustment { get; set; } = 0;          

   
    [NotMapped]
    public decimal RemainingBalance => OpeningBalance + CarriedForward + Adjustment - UsedDays;
}
