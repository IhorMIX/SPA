using SPA.DAL.Entity;

namespace SPA.DAL.Repositories.Interfaces;

public interface IUserRepository : IBasicRepository<User>
{
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(User user, CancellationToken cancellationToken = default);
}