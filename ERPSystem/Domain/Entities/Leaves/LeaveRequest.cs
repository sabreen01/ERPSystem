using System.ComponentModel.DataAnnotations.Schema;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Enums;

namespace ERPSystem.Domain.Entities.Leaves;

[Table(name: "LeaveRequest", Schema = "Leaves")]
public class LeaveRequest : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public Guid LeaveTypeId { get; set; }
    public LeaveType? LeaveType { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DaysCount { get; set; }
    public string? Reason { get; set; }

    public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;

    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }
    
    public ICollection<LeaveRequestDay> LeaveRequestDays { get; set; } = new List<LeaveRequestDay>();
}
