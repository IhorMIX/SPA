namespace SPA.Web.Models;

public class UserAuthorizeModel
{
    public string UserName { get; set; } = null!;
    
    public string Password { get; set; } = null!;

    public bool IsNeedToRemember { get; set; }
}