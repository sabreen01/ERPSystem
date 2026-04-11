using ERPSystem.Application.Features.Auth.Register.DTOs;
using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Auth.Register.Commands;
public record RegisterUserCommand(RegisterDTO RegisterData) : IRequest<bool>;

public class RegisterUserHandler(
    IRepository<User> userRepository, 
    IRepository<Role> roleRepository, 
    IRepository<UserRole> userRoleRepository) 
    : IRequestHandler<RegisterUserCommand, bool>
{
    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var data = request.RegisterData;

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = data.UserName,
            FirstName = data.FirstName,
            LastName = data.LastName,
            Email = data.Email,
            Password = data.Password, 
            EmailConfirmed = false
        };

        var managerRoleName = nameof(UserRoles.Manager);
        var managerRole = await roleRepository.GetAll(r => r.Name == managerRoleName)
            .FirstOrDefaultAsync(cancellationToken);

        if (managerRole == null) return false;

        var userRole = new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = newUser.Id,
            RoleId = managerRole.Id
        };
    
      
        userRepository.Add(newUser);
        userRoleRepository.Add(userRole);
    
        await userRepository.SaveChangesAsync(cancellationToken);
    
        return true;
    }
}
