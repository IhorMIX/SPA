namespace SPA.BLL.Models;

public class AuthorizationInfoModel: BaseModel
{
    public string RefreshToken { get; set; } = null!;

    public DateTime? ExpiredDate { get; set; }
}