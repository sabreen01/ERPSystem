using ERPSystem.Domain.Entities.Identity;

namespace ERPSystem.Application.Interfaces;

public interface IAuthService
{
    string GenerateToken(Guid userId);
}