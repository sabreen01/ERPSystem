using ERPSystem.Application.Common;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Entities.AttendanceManagment;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Infrastructure;

public class AppDBContext(DbContextOptions<AppDBContext> options , UserState userState) : DbContext(options)
{
    public DbSet<User>  Users { get; set; }
    public DbSet<Role>  Roles { get; set; }
    public DbSet<AccessToken>   AccessTokens { get; set; }
    public DbSet<UserRole>  UserRoles { get; set; }
    
    public DbSet<Department>  Departments { get; set; }
    public DbSet<Position>  Positions { get; set; }
    public DbSet<Branch> Branches { get; set; }
    
    public DbSet<Attendance> Attendances { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<UserRole>().HasQueryFilter(ur => !ur.IsDeleted);
        modelBuilder.Entity<Department>().HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<Position>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Branch>().HasQueryFilter(b => !b.IsDeleted);
        modelBuilder.Entity<Attendance>().HasQueryFilter(a => !a.IsDeleted);
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
                if (currentUserId != Guid.Empty) entry.Entity.CreatedBy = currentUserId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                if (currentUserId != Guid.Empty) entry.Entity.UpdatedBy = currentUserId;
            }
          
            else if (entry.State == EntityState.Deleted)
            {
              
                entry.State = EntityState.Modified; 
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
                if (currentUserId != Guid.Empty) entry.Entity.DeletedBy = currentUserId;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}