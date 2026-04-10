using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ERPSystem.Application.Interfaces;
using ERPSystem.Domain.Entities.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ERPSystem.Application.Services;

public class AuthService (IConfiguration configuration) : IAuthService
{

    public string GenerateToken(User userId ,  out string JwtId )
    {
        JwtId = Guid.NewGuid().ToString();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, JwtId),
            new Claim(JwtRegisteredClaimNames.Email, userId.Email)
        };
        
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