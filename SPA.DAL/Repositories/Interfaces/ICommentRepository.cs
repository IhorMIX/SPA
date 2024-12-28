using SPA.DAL.Entity;

namespace SPA.DAL.Repositories.Interfaces;

public interface ICommentRepository: IBasicRepository<Comment>
{
    Task<IEnumerable<Comment>> GetRepliesAsync(int commentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Comment?>> GetCommentsTreeAsync(int commentId, CancellationToken cancellationToken = default);
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Comment comment, CancellationToken cancellationToken = default);
}