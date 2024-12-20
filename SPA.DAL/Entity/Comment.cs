namespace SPA.DAL.Entity;

public class Comment : BaseEntity
{
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int? ParentCommentId { get; set; }
    public Comment ParentComment { get; set; } = null!;
    public ICollection<Comment> Replies { get; set; } = null!;
    public ICollection<Attachment> Attachments { get; set; } = null!;
}