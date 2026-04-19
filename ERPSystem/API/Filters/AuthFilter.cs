using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ERPSystem.Application.Common;
using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace ERPSystem.API.Filters;

public class AuthFilter(IRepository<AccessToken> repository , IConfiguration configuration, UserState userState) : IAuthorizationFilter
{
   
    public void OnAuthorization(AuthorizationFilterContext context)
{
    var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

    if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
    {
        context.Result = new UnauthorizedResult();
        return;
    }

    var jwtTokenRaw = authHeader.Substring("Bearer ".Length).Trim();

    try
    {
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
            ValidateIssuer = true,
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["JwtSettings:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var principal = handler.ValidateToken(jwtTokenRaw, validationParameters, out SecurityToken validatedToken);
        var jwtToken = (JwtSecurityToken)validatedToken;
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value 
                          ?? jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var jwtId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        
        var roleClaims = jwtToken.Claims
            .Where(c => c.Type == "role" || c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value 
                         ?? jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        if (string.IsNullOrEmpty(jwtId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var accessTokenInDb = repository.GetAll(t => t.TokenId == jwtId).FirstOrDefault();
        
        if (accessTokenInDb == null || accessTokenInDb.ExpiredAt < DateTime.UtcNow)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // userState.UserId = accessTokenInDb.UserId;
        userState.UserId = Guid.Parse(userIdClaim);
        userState.Email = emailClaim;
        userState.Roles = roleClaims;

        context.HttpContext.User = principal;
    }
    catch (Exception)
    {
        context.Result = new UnauthorizedResult();
    }
}
}