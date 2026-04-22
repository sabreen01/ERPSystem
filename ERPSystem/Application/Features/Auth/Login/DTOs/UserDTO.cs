namespace ERPSystem.Application.Features.Auth.Login.DTOs;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
}