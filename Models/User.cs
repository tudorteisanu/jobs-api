using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public int Id { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
}