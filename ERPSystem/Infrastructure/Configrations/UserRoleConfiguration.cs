using ERPSystem.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPSystem.Infrastructure.Configrations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ConfigureBaseEntity();
        builder.HasKey(ur => ur.Id);
        builder.HasOne(e => e.User)
               .WithMany(e => e.UserRoles)
               .HasForeignKey(e => e.UserId);
        
        builder.HasOne(e => e.Role)
               .WithMany(e => e.UserRoles)
               .HasForeignKey(e => e.RoleId);
        
        builder.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();
    }
}