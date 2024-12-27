using SPA.BLL.Models;

namespace SPA.BLL.Services.Interfaces;

public interface ICommentService : IBasicService<CommentModel>
{
    Task AddAsync(CommentModel commentModel,UserModel userModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(int commentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CommentModel>> GetRepliesAsync(int commentId, CancellationToken cancellationToken = default);
    Task<CommentModel> GetCommentsTreeAsync(int commentId, CancellationToken cancellationToken = default);
}