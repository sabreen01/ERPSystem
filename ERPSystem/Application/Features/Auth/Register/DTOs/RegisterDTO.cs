namespace ERPSystem.Application.Features.Auth.Register.DTOs;

public record RegisterDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

// {
// "UserName": "Farah Essam",
//    "FirstName" : "Farah",
//     "LastName" : "Essam",
//     "Email" : "Farah@123",
//     "Password" : "farah121198"
// }