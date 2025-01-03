using SPA.BLL.Models;

namespace SPA.BLL.Services.Interfaces;

public interface ICommentService : IBasicService<CommentModel>
{
    Task<CommentModel> AddCommentAsync(string text, int? parentCommentId, int userId,
        CancellationToken cancellationToken = default);
    Task DeleteCommentAsync(int commentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CommentModel>> GetRepliesAsync(int commentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CommentModel>> GetCommentsTreeAsync(int commentId, CancellationToken cancellationToken = default);
}