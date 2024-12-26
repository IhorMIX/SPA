using AutoMapper;
using SPA.BLL.Exceptions;
using SPA.BLL.Models;
using SPA.BLL.Services.Interfaces;
using SPA.DAL.Entity;
using SPA.DAL.Repositories.Interfaces;

namespace SPA.BLL.Services;

public class CommentService(ICommentRepository commentRepository,IUserRepository userRepository,  IMapper mapper) : ICommentService
{
    public async Task<CommentModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var commentDb = await commentRepository.GetByIdAsync(id, cancellationToken);
        if (commentDb == null)
        {
            throw new CommentNotFoundException($"Comment with this Id {id} not found");
        }
        var commentModel = mapper.Map<CommentModel>(commentDb);
        return commentModel;
    }

    public async Task AddAsync(CommentModel commentModel,UserModel userModel, CancellationToken cancellationToken = default)
    {
        var commentDb = await commentRepository.GetByIdAsync(commentModel.Id, cancellationToken);
        if (commentDb == null)
        {
            throw new CommentNotFoundException($"Comment with this Id {commentModel.Id} not found");
        }
        var userDb = await userRepository.GetByIdAsync(userModel.Id, cancellationToken);
        if (userDb == null)
        {
            throw new UserNotFoundException($"User with this Id {userModel.Id} not found");
        }
        commentDb.CreatedAt = DateTime.UtcNow;
        await commentRepository.AddAsync(commentDb, cancellationToken);
    }

    public async Task DeleteAsync(int commentId, CancellationToken cancellationToken = default)
    {
        var commentDb = await commentRepository.GetByIdAsync(commentId, cancellationToken);
        if (commentDb == null)
        {
            throw new CommentNotFoundException($"Comment with this Id {commentId} not found");
        }
        await commentRepository.DeleteAsync(commentDb, cancellationToken);
    }
}