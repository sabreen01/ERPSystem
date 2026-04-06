using ERPSystem.Application.Features.Auth.Login.Commands;
using ERPSystem.Application.Features.Auth.Login.Queries;
using ERPSystem.Application.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Auth.Login.Orchestrator;

public record LoginOrchestrator (string Email ,string Password) :IRequest<string>;
public class LoginOrchestratorHandler(IMediator mediator , IAuthService authService) :IRequestHandler<LoginOrchestrator, string>
{
    public async Task<string> Handle(LoginOrchestrator request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserByEmailQuery(request.Email), cancellationToken);

        if (user is null)
        {
            return null;
        }

        if (user.Password != request.Password)
        {
            return null;
        }

        var token =  authService.GenerateToken(user.UserId);
        await mediator.Send(new AccessTokenCommand(token , DateTime.UtcNow.AddDays(1), user.UserId));
        return token;
    }
}