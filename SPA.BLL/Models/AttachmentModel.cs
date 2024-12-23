namespace SPA.BLL.Models;

public class AttachmentModel : BaseModel
{
    public string FileName { get; set; } = null!;
    public string FileType { get; set; } = null!;
    public int CommentId { get; set; }
    public CommentModel Comment { get; set; } = null!;
}