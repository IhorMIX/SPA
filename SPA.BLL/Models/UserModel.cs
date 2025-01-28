namespace SPA.BLL.Models;

public class UserModel : BaseModel
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? HomePage { get; set; }
    public AuthorizationInfoModel? AuthorizationInfo { get; set; }
    public ICollection<CommentModel> Comments { get; set; } = null!;
}