using ERPSystem.Application.Features.Auth.Login.DTOs;
using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Auth.Login.Queries;

public record GetUserByEmailQuery(string Email) : IRequest<User>;
public class GetUserByEmailQueryHandler(IRepository<User> repository) : IRequestHandler<GetUserByEmailQuery, User>
{
    
    public async Task<User?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAll(p => p.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);
    }
}