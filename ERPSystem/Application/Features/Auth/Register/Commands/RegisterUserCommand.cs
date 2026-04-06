using ERPSystem.Application.Features.Auth.Register.DTOs;
using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Auth.Register.Commands;
public record RegisterUserCommand(RegisterDTO RegisterData) : IRequest<bool>;

public class RegisterUserHandler(IRepository<User> repository) 
    : IRequestHandler<RegisterUserCommand, bool>
{
    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
       
        var data = request.RegisterData;
        var newUser = new User
        {
            UserName = data.UserName,
            FirstName = data.FirstName,
            LastName = data.LastName,
            Email = data.Email,
            Password = data.Password, 
            EmailConfirmed = false
        };
         repository.Add(newUser);
         await repository.SaveChangesAsync(cancellationToken);
         return true;
    }
}
