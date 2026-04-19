using ERPSystem.Application.Features.Auth.Register.DTOs;
using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Auth.Register.Commands;

public record RegisterUserCommand(RegisterDTO RegisterData) : IRequest<Guid>;

public class RegisterUserHandler(
    IRepository<User> userRepository, 
    IRepository<Role> roleRepository, 
    IRepository<UserRole> userRoleRepository) 
    : IRequestHandler<RegisterUserCommand, Guid> 
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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

        var RoleName = nameof(UserRoles.Employee);
        var Role = await roleRepository.GetAll(r => r.Name == RoleName)
            .FirstOrDefaultAsync(cancellationToken);

        if (Role == null) return Guid.Empty; 

        var userRole = new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = newUser.Id,
            RoleId = Role.Id
        };
    
        userRepository.Add(newUser);
        userRoleRepository.Add(userRole);
    
        await userRepository.SaveChangesAsync(cancellationToken);
        return newUser.Id; 
    }
}