namespace SPA.BLL.Models;

public class AttachmentModel : BaseModel
{
    public string FileURL { get; set; }
    public int? CommentId { get; set; }
    public CommentModel? Comment { get; set; } = null!;
}