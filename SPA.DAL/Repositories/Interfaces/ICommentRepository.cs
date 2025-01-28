using SPA.DAL.Entity;

namespace SPA.DAL.Repositories.Interfaces;

public interface ICommentRepository: IBasicRepository<Comment>
{
    // IQueryable<Comment> GetRepliesAsync(int commentId);
    IQueryable<Comment> GetAllParentCommentsAsync();
    Task<IEnumerable<Comment>> GetAllTreesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Comment>> GetTreeAsync(int commentId, CancellationToken cancellationToken = default);
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Comment comment, CancellationToken cancellationToken = default);
}