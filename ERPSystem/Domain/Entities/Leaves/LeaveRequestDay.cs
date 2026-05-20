namespace ERPSystem.Domain.Entities.Leaves;

public class LeaveRequestDay : BaseEntity
{
    public Guid LeaveRequestId { get; set; }
    public LeaveRequest LeaveRequest { get; set; } = null!;
    
    public DateTime Date { get; set; }
}
