using System.Text.Json.Serialization;
using SPA.BLL.Models;

namespace SPA.Web.Models;

public class CommentViewModel
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int? ParentCommentId { get; set; }
    public int UserId { get; set; }
    
    // [JsonIgnore]
    // public ICollection<CommentViewModel> Replies { get; set; } = new List<CommentViewModel>();
}
