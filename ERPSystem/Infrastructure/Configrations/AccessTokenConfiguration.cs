using ERPSystem.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPSystem.Infrastructure.Configrations;

public class AccessTokenConfiguration : IEntityTypeConfiguration<AccessToken>
{
    public void Configure(EntityTypeBuilder<AccessToken> builder)
    {
        builder.ConfigureBaseEntity();
        builder.Property(t => t.TokenId).IsRequired().HasMaxLength(255);
        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId);
        builder.HasOne(t => t.Role)
            .WithMany()
            .HasForeignKey(t => t.RoleId);
    }
}