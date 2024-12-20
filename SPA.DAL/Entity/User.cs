namespace SPA.DAL.Entity;

public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string HomePage { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
}