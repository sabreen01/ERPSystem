using ERPSystem.Application.Interfaces;
using ERPSystem.Domain.Entities.Identity;

namespace ERPSystem.Application.Services;

public class AuthService : IAuthService
{

    public string GenerateToken(Guid userId)
    {
        var Token = Guid.NewGuid().ToString();
        return  Token;
    }
}