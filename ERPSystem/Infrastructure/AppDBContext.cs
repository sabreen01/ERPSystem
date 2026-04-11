using ERPSystem.Application.Common;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Infrastructure;

public class AppDBContext(DbContextOptions<AppDBContext> options , UserState userState) : DbContext(options)
{
    public DbSet<User>  Users { get; set; }
    public DbSet<Role>  Roles { get; set; }
    public DbSet<AccessToken>   AccessTokens { get; set; }
    public DbSet<UserRole>  UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<UserRole>().HasQueryFilter(ur => !ur.IsDeleted);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            var currentUserId = userState.UserId;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            
                if (currentUserId != Guid.Empty)
                {
                    entry.Entity.CreatedBy = currentUserId;
                }
            }
          
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;

                if (currentUserId != Guid.Empty)
                {
                    entry.Entity.UpdatedBy = currentUserId;
                }
            }
            
            else if (entry.State == EntityState.Deleted)
            {
                entry.Entity.DeletedAt = DateTime.UtcNow;
                if (currentUserId != Guid.Empty)
                {
                    entry.Entity.DeletedBy = currentUserId;
                }
            }
        }

        
        return base.SaveChangesAsync(cancellationToken);
    }
   
}