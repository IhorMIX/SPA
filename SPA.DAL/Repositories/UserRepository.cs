using Microsoft.EntityFrameworkCore;
using SPA.DAL.Entity;
using SPA.DAL.Repositories.Interfaces;

namespace SPA.DAL.Repositories;

public class UserRepository(SPADbContext spaDbContext) : IUserRepository
{
    private readonly SPADbContext _spaDbContext = spaDbContext;

    public IQueryable<User> GetAll()
    {
        return _spaDbContext.Users.Include(i => i.AuthorizationInfo).AsQueryable();
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await spaDbContext.Users.Include(i => i.AuthorizationInfo).FirstOrDefaultAsync(i => i.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await spaDbContext.Users.AddAsync(user, cancellationToken);
        await spaDbContext.SaveChangesAsync(cancellationToken);
        return user;
    }
    public async Task DeleteUserAsync(User user, CancellationToken cancellationToken = default)
    {
        spaDbContext.Users.Remove(user);
        await spaDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        spaDbContext.Users.Update(user);
        await spaDbContext.SaveChangesAsync(cancellationToken);
    }
}