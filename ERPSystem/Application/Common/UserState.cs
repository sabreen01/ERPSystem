namespace ERPSystem.Application.Common;

public class UserState
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}