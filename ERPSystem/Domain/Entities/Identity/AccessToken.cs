using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Domain.Entities.Identity;
[Table(name : "AccessToken" , Schema = "Identity")]
public class AccessToken : BaseEntity
{
    public string TokenId { get; set; }
    public DateTime ExpiredAt { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
    
}