namespace SPA.DAL.Repositories.Interfaces;

public interface IBasicRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();

    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}