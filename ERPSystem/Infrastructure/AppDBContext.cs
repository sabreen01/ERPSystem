using ERPSystem.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Infrastructure;

public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
{
    public DbSet<User>  Users { get; set; }
    public DbSet<Role>  Roles { get; set; }
    public DbSet<AccessToken>   AccessTokens { get; set; }
    public DbSet<UserRole>  UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
}