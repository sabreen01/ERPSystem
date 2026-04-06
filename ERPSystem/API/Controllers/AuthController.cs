using ERPSystem.API.Filters;
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
public class AuthController (IMediator mediator , IRepository<AccessToken> repository) : ControllerBase
{
    
    [HttpPost("register")]
    public async Task<ActionResult> Register( RegisterDTO dto)
    {
        var data = await mediator.Send(new RegisterOrchestrator(dto));
        return data ? Ok() : BadRequest();


    }
    
    [HttpPost("login")]
    public async Task<RequestResult<string>> Login([FromBody] UserDTO loginDto)
    {
        var data = await mediator.Send(new LoginOrchestrator(loginDto.Email, loginDto.Password));
        return new RequestResult<string>(IsSuccess: true , Message: "Login successful" , Data: data);
    }
    [ServiceFilter(typeof(AuthFilter))]
    [HttpGet("Auth")]
   
    public async Task<ActionResult> GetAuth()
    {
        return Ok("hello world");
    }
    
    
    
    
}