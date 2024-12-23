namespace SPA.Web.Models;

public class UserCreateModel
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string HomePage { get; set; } = null!;
}