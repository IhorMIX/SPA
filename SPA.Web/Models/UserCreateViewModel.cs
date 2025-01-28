namespace SPA.Web.Models;

public class UserCreateViewModel
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? HomePage { get; set; }
}