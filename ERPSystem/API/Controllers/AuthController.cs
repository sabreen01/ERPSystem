using ERPSystem.Application.Features.Auth.Login.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController (IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get()
    {
       var result = await  mediator.Send(new GetUserByEmailQuery("asd"));
        
        return Ok(result);
    }
    
    
}