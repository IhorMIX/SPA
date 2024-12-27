namespace SPA.Web.Models;

public class CommentCreateModel
{
    public string Text { get; set; } = null!;
    public int? ParentCommentId { get; set; }
    public int UserId { get; set; }
}
