using System.ComponentModel.DataAnnotations.Schema;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Entities.HR;

namespace ERPSystem.Domain.Entities.AttendanceManagment;
[Table(name : "Attendance" , Schema = "AttendanceManagment")]
public class Attendance : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }
    
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    
    public double? OvertimeHours { get; set; }
    public OvertimeStatus OvertimeStatus { get; set; } = OvertimeStatus.None;
    public string? Notes { get; set; }
}