namespace SPA.BLL.Models;

public class CommentModel : BaseModel
{
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public UserModel User { get; set; } = null!;
    public int? ParentCommentId { get; set; }
    public CommentModel ParentComment { get; set; } = null!;
    public ICollection<CommentModel> Replies { get; set; } = null!;
    public ICollection<AttachmentModel> Attachments { get; set; } = null!;
}