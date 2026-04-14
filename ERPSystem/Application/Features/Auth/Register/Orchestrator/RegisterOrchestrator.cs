using ERPSystem.Application.Features.Auth.Login.Queries;
using ERPSystem.Application.Features.Auth.Register.Commands;
using ERPSystem.Application.Features.Auth.Register.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Application.Interfaces;
using ERPSystem.Application.Services;
using MediatR;

namespace ERPSystem.Application.Features.Auth.Register.Queries;

public record RegisterOrchestrator(RegisterDTO registerDto) : IRequest<RequestResult<bool>>;
public class RegisterOrchestratorHandler (IMediator mediator, IPasswordService passwordService) 
    :IRequestHandler<RegisterOrchestrator,RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(RegisterOrchestrator request, CancellationToken cancellationToken)
    {
        var dto = request.registerDto;

        var existingUser = await mediator.Send(new GetUserByEmailQuery(dto.Email), cancellationToken);

        if (existingUser != null)
        {
            return new RequestResult<bool>(Data: false, IsSuccess: false, Message: "Already existed Email");
        }
        
        var hashedPassword = passwordService.HashPassword(request.registerDto.Password);
        var command = new RegisterUserCommand(dto with { Password = hashedPassword });
        var isCreated = await mediator.Send(command, cancellationToken);

        if (!isCreated)
        {
            return RequestResult<bool>.Failure("There was an error while creating the account.");
        }

        return RequestResult<bool>.Success(true, "Registration successful.");
        
    }
}