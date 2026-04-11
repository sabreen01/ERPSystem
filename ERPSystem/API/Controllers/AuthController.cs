using ERPSystem.API.Filters;
using ERPSystem.Application.Common;
using ERPSystem.Application.Features.Auth.Login.DTOs;
using ERPSystem.Application.Features.Auth.Login.Orchestrator;
using ERPSystem.Application.Features.Auth.Login.Queries;
using ERPSystem.Application.Features.Auth.Register.Commands;
using ERPSystem.Application.Features.Auth.Register.DTOs;
using ERPSystem.Application.Features.Auth.Register.Queries;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController (IMediator mediator , IRepository<AccessToken> repository , UserState userState) : BaseController
{
    
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDTO dto)
    {
        return HandleResult(await mediator.Send(new RegisterOrchestrator(dto)));
        
    }
    
    
    
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserDTO loginDto)
    {
        return HandleResult(await mediator.Send(new LoginOrchestrator(loginDto.Email, loginDto.Password)));
    }
    
    
    
    [ServiceFilter(typeof(AuthFilter))]
    [HttpGet("Auth")]
   
    public async Task<ActionResult> GetAuth()
    {
        // var userId = HttpContext.Items["UserId"] as Guid?;
        var userId = userState.UserId;
       
        if (userId == Guid.Empty)
        {
            return Unauthorized("no id founded");
        }

        return Ok(new 
        { 
            Message = "welcome ",
            YourId = userId ,
            YourEmail = userState.Email,
            YourRole  =  userState.Roles
        });
    }
    
    }
    
