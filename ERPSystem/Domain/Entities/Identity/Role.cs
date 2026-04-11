using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Domain.Entities.Identity;

[Table(name : "Role" , Schema = "Identity")]
public class Role : BaseEntity
{
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    
}