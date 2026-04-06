using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ERPSystem.API.Filters;

public class AuthFilter(IRepository<AccessToken> repository) : IAuthorizationFilter
{
    //
    // public new void  OnActionExecuting(ActionExecutingContext context)
    // {
    //     var token = string.Empty;
    //     var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
    //     if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
    //     {
    //         token = authHeader.Substring("Bearer ".Length).Trim();
    //     }
    //     else
    //     {
    //         return;
    //     }
    //
    //   var accessToken =   repository.GetAll(t => t.Token == token).FirstOrDefault();
    //   if (accessToken == null)
    //   {
    //       return;
    //   }
    //
    // }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = string.Empty;
        var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            token = authHeader.Substring("Bearer ".Length).Trim();
        }
        else
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var accessToken =   repository.GetAll(t => t.Token == token).FirstOrDefault();
        if (accessToken == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}