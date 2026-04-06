using ERPSystem.Application.Features.Auth.Login.Queries;
using ERPSystem.Application.Features.Auth.Register.Commands;
using ERPSystem.Application.Features.Auth.Register.DTOs;
using MediatR;

namespace ERPSystem.Application.Features.Auth.Register.Queries;

public record RegisterOrchestrator(RegisterDTO registerDto) : IRequest<bool>;
public class RegisterOrchestratorHandler (IMediator mediator) :IRequestHandler<RegisterOrchestrator, bool>
{
    public async Task<bool> Handle(RegisterOrchestrator request, CancellationToken cancellationToken)
    {
        var dto = request.registerDto;

        var existingUser = await mediator.Send(new GetUserByEmailQuery(dto.Email), cancellationToken);

        if (existingUser != null)
        {
            return false; 
        }

        var isCreated = await mediator.Send(new RegisterUserCommand(dto), cancellationToken);
        return isCreated;
    }
}