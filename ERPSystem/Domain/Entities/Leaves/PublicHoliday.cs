using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Domain.Entities.Leaves;

[Table(name: "PublicHolidays", Schema = "Leaves")]
public class PublicHoliday : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsActive { get; set; } = true;
}
