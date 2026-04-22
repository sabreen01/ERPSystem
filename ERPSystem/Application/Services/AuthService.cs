using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ERPSystem.Application.Interfaces;
using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Enums;
using Microsoft.IdentityModel.Tokens;

namespace ERPSystem.Application.Services;

public class AuthService (IConfiguration configuration) : IAuthService
{

    public string GenerateToken(User user, out string jwtId)
    {
        jwtId = Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        if (user.UserRoles != null)
        {
            foreach (var userRole in user.UserRoles)
            {
                if (Enum.TryParse<UserRoles>(userRole.Role.Name, out var roleEnum))
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleEnum.ToString()));
                }
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), 
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JwtSettings:DurationInDays"])),
            Issuer = configuration["JwtSettings:Issuer"],
            Audience = configuration["JwtSettings:Audience"],
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }
}