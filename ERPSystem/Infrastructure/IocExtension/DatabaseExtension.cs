using ERPSystem.Domain.Interfaces;
using ERPSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Infrastructure.IocExtension;

public static class DatabaseExtension
{
    public static void AddDatabaseExtension(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDBContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("connectionString"));
            
        });
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}