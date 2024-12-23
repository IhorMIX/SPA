namespace SPA.Web.Models;

public class UserViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string HomePage { get; set; } = null!;
}