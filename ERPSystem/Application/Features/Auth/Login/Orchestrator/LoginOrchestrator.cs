using ERPSystem.Application.Features.Auth.Login.Commands;
using ERPSystem.Application.Features.Auth.Login.Queries;
using ERPSystem.Application.Helper.models;
using ERPSystem.Application.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Auth.Login.Orchestrator;

public record LoginOrchestrator (string Email ,string Password) : IRequest<RequestResult<string>>;
public class LoginOrchestratorHandler(
    IMediator mediator, 
    IAuthService authService, 
    IPasswordService passwordService)
    : IRequestHandler<LoginOrchestrator, RequestResult<string>> 
{
    public async Task<RequestResult<string>> Handle(LoginOrchestrator request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserByEmailQuery(request.Email), cancellationToken);

        if (user is null || !passwordService.VerifyPassword(request.Password, user.Password))
        {
            return RequestResult<string>.Failure("The email or password is wrong!");
        }
        
        var userRoleId = user.UserRoles.Select(ur => ur.RoleId).FirstOrDefault();

        if (userRoleId == Guid.Empty) 
        {
            return RequestResult<string>.Failure("user hasn't Role");
        }

        var token = authService.GenerateToken(user , out string jwtId);
        await mediator.Send(new AccessTokenCommand(
            Token: jwtId,
            UserId: user.Id,
            RoleId: userRoleId,
            ExpiresAt: DateTime.UtcNow.AddDays(1)
        ),cancellationToken);
        return RequestResult<string>.Success(token, "Login successful");
    }
}