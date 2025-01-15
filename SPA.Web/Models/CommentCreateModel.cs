using SPA.BLL.Models;

namespace SPA.Web.Models;

public class CommentCreateModel
{
    public string Text { get; set; } = null!;
    public int? ParentCommentId { get; set; }
    public ICollection<AttachmentModel> Attachments { get; set; } = null!;
}

