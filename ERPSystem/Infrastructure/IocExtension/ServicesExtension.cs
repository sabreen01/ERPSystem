using ERPSystem.API.Filters;
using ERPSystem.Application.Interfaces;
using ERPSystem.Application.Services;

namespace ERPSystem.Infrastructure.IocExtension;

public static class ServicesExtension
{
    public static void UseService(this IServiceCollection services , IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<AuthFilter>();
        
    }
    
}