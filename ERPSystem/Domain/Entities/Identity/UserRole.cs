namespace ERPSystem.Domain.Entities.Identity;

public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    
}