using SPA.BLL.Models;

namespace SPA.BLL.Services.Interfaces;

public interface IUserService: IBasicService<UserModel>
{
    Task CreateUserAsync(UserModel user, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserModel> UpdateUserAsync(int id, UserModel user, CancellationToken cancellationToken = default);
    Task<UserModel> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<UserModel?> GetUserByLoginAndPasswordAsync(string userName, string password, CancellationToken cancellationToken = default);
    Task AddAuthorizationValueAsync(UserModel user, string refreshToken, DateTime? expiredDate = null,
        CancellationToken cancellationToken = default);
    Task<UserModel?> GetUserByLogin(string userName, CancellationToken cancellationToken = default);
    
    Task LogOutAsync(int userId, CancellationToken cancellationToken = default);
}