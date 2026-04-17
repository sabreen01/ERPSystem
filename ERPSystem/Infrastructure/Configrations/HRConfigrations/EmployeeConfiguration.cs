using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPSystem.Infrastructure.Configrations.HRConfigrations;

public class EmployeeConfiguration: IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ConfigureBaseEntity();
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.NationalId).IsRequired().HasMaxLength(20);
        builder.Property(e => e.EmployeeNo).IsRequired().HasMaxLength(20);
        builder.Property(e => e.PersonalPhone).IsRequired().HasMaxLength(20);
        builder.HasIndex(e => e.NationalId).IsUnique();
        builder.HasIndex(e => e.EmployeeNo).IsUnique();
        
        builder.Property(e => e.Gender)
            .HasConversion(
                v => v.ToString(),
                v => (Gender)Enum.Parse(typeof(Gender), v))
            .HasMaxLength(10);

        builder.Property(e => e.ContractType)
            .HasConversion(
                v => v.ToString(),
                v => (ContractType)Enum.Parse(typeof(ContractType), v))
            .HasMaxLength(20);
        
        builder.Property(e => e.BasicSalary)
            .HasPrecision(18, 2);

        builder.HasOne(e => e.Branch)
            .WithMany()
            .HasForeignKey(e => e.BranchId);
        builder.HasOne(e => e.Department)
            .WithMany()
            .HasForeignKey(e => e.DepartmentId);

        builder.HasOne(e => e.Position)
            .WithMany()
            .HasForeignKey(e => e.PositionId);
    }
    
}