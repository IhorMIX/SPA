using SPA.BLL.Models;

namespace SPA.BLL.Services.Interfaces;

public interface IUserService: IBasicService<UserModel>
{
    Task<UserModel> CreateUserAsync(string username, string email,string homePage, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default);
}