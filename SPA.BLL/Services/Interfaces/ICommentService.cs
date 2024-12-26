using SPA.BLL.Models;
using SPA.DAL.Entity;

namespace SPA.BLL.Services.Interfaces;

public interface ICommentService : IBasicService<CommentModel>
{
    Task AddAsync(CommentModel commentModel,UserModel userModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(int commentId, CancellationToken cancellationToken = default);
}