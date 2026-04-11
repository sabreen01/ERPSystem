using ERPSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPSystem.Infrastructure.Configrations;

public static class EntityConfigurationExtension
{
    public static void ConfigureBaseEntity<T>(this EntityTypeBuilder<T> builder) where T : BaseEntity
    
        {
            
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()"); 

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("now()"); 

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.Property(e => e.DeletedAt)
                .IsRequired(false);

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.CreatedBy).IsRequired(false);
            builder.Property(e => e.UpdatedBy).IsRequired(false);
            builder.Property(e => e.DeletedBy).IsRequired(false);

            builder.HasIndex(e => e.IsDeleted);
            builder.HasIndex(e => e.CreatedAt);
        }
            
            
}
    
