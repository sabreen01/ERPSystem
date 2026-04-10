using System.IdentityModel.Tokens.Jwt;
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
   
    public void OnAuthorization(AuthorizationFilterContext context )
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
            var jwtId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
          
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
            
            var userState = context.HttpContext.RequestServices.GetService(typeof(UserState)) as UserState;
            userState.UserId = accessTokenInDb.UserId;
            userState.Email = emailClaim;
            
            
            // context.HttpContext.Items["UserId"] = accessTokenInDb.UserId;
        }
        catch (Exception)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}