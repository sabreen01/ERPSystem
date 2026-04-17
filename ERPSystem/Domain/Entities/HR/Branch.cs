using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Domain.Entities.HR;
[Table(name : "Branch" , Schema = "HR")]
public class Branch:BaseEntity
{
    public string Name { get; set; } = string.Empty; 
    public string Location { get; set; } = string.Empty; 
    public bool IsActive { get; set; } = true;
}