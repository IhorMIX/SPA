namespace SPA.DAL.Entity;

public class AuthorizationInfo: BaseEntity
{
    public string RefreshToken { get; set; }  = null!;

    public DateTime? ExpiredDate { get; set; }
    
    public int UserId { get; set; }

    public User User { get; set; } = null!;
}