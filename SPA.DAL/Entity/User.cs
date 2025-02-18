namespace SPA.DAL.Entity;

public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? HomePage { get; set; }
    public int AuthorizationInfoId { get; set; }
    public AuthorizationInfo? AuthorizationInfo { get; set; }
    public ICollection<Comment> Comments { get; set; } = null!;
    
}