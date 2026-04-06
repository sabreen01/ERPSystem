using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Domain.Entities.Identity;
[Table(name : "AccessToken" , Schema = "Identity")]
public class AccessToken : BaseEntity
{
    public string Token { get; set; }
    public DateTime ExpiredAt { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
}