using ERPSystem.Application.Features.Auth.Login.Queries;
using ERPSystem.Application.Features.Auth.Register.Commands;
using ERPSystem.Application.Features.Auth.Register.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Application.Interfaces;
using ERPSystem.Application.Services;
using MediatR;

namespace ERPSystem.Application.Features.Auth.Register.Queries;

public record RegisterOrchestrator(RegisterDto RegisterDto) : IRequest<RequestResult<Guid>>;
public class RegisterOrchestratorHandler (IMediator mediator, IPasswordService passwordService) 
    : IRequestHandler<RegisterOrchestrator, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(RegisterOrchestrator request, CancellationToken cancellationToken)
    {
        var dto = request.RegisterDto;

        var existingUser = await mediator.Send(new GetUserByEmailQuery(dto.Email), cancellationToken);
        if (existingUser != null)
        {
            return RequestResult<Guid>.Failure("Email already exists");
        }
       
        var hashedPassword = passwordService.HashPassword(dto.Password);
       
        var command = new RegisterUserCommand(dto with { Password = hashedPassword });
        var userId = await mediator.Send(command, cancellationToken);

        if (userId == Guid.Empty)
        {
            return RequestResult<Guid>.Failure("there is an error");
        }

        return RequestResult<Guid>.Success(userId, "Register successful");
    }
}