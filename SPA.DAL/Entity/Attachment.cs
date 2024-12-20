namespace SPA.DAL.Entity;

public class Attachment : BaseEntity
{
    public string FileName { get; set; } = null!;
    public string FileType { get; set; } = null!;
    public int CommentId { get; set; }
    public Comment Comment { get; set; } = null!;
}