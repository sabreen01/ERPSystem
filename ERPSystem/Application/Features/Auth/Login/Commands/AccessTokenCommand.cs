using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Auth.Login.Commands;

public record AccessTokenCommand(string Token, DateTime ExpiresAt , Guid UserId) : IRequest;
public class AccessTokenCommandHandler(IRepository<AccessToken> repository) : IRequestHandler<AccessTokenCommand>
{
    public async Task Handle(AccessTokenCommand request, CancellationToken cancellationToken)
    {
        var newToken = new AccessToken
        {
            Token = request.Token,
            UserId = request.UserId,
            ExpiredAt = request.ExpiresAt
        };
        repository.Add(newToken);
        await repository.SaveChangesAsync(cancellationToken);
        
    }
}