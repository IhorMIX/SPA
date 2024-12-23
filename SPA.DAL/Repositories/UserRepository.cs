using Microsoft.EntityFrameworkCore;
using SPA.DAL.Entity;
using SPA.DAL.Repositories.Interfaces;

namespace SPA.DAL.Repositories;

public class UserRepository(SPADbContext spaDbContext) : IUserRepository
{
    private readonly SPADbContext _spaDbContext = spaDbContext;

    public IQueryable<User> GetAll()
    {
        return _spaDbContext.Users.AsQueryable();
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _spaDbContext.Users.FirstOrDefaultAsync(i => i.Id == id, cancellationToken: cancellationToken);
    }

    public async Task CreateUser(User user, CancellationToken cancellationToken = default)
    {
        await _spaDbContext.Users.AddAsync(user, cancellationToken);
        await _spaDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _spaDbContext.Users.Update(user);
        await _spaDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _spaDbContext.Users.Remove(user);
        await _spaDbContext.SaveChangesAsync(cancellationToken);
    }
}