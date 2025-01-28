namespace SPA.DAL.Entity;

public class Attachment : BaseEntity
{
    public string FileURL { get; set; } = null!;
    public int? CommentId { get; set; }
    public Comment? Comment { get; set; }
}
