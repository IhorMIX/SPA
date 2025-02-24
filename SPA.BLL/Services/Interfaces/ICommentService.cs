using SPA.BLL.Models;

namespace SPA.BLL.Services.Interfaces;

public interface ICommentService : IBasicService<CommentModel>
{
    Task<CommentModel> AddCommentAsync(CommentModel commentModel, int userId,
        CancellationToken cancellationToken = default);
    Task DeleteCommentAsync(int commentId, CancellationToken cancellationToken = default);

    Task<PaginationResultModel<CommentModel>> GetAllParentCommentsAsync(
        PaginationModel pagination,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<CommentModel>> GetTreeByCommentIdAsync(int commentId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<CommentModel>> GetAllCommentTreesAsync(
        PaginationModel pagination,
        CancellationToken cancellationToken = default);
}