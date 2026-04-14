using ERPSystem.Domain.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPSystem.Infrastructure.Configrations.HRConfigrations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ConfigureBaseEntity();
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Code).IsRequired().HasMaxLength(20);
       
        builder.HasIndex(p => p.Code).IsUnique();

        builder.Property(p => p.MinSalary).HasPrecision(18, 2);
        builder.Property(p => p.MaxSalary).HasPrecision(18, 2);

        builder.HasOne(p => p.Department)
            .WithMany(d => d.Positions)
            .HasForeignKey(p => p.DepartmentId);
    }
}