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

    public async Task<CommentModel> AddCommentAsync(string text,int? parentCommentId,int userId, CancellationToken cancellationToken = default)
    {
        // var userDb = await userRepository.GetByIdAsync(userModel.Id, cancellationToken);
        // if (userDb == null)
        // {
        //     throw new UserNotFoundException($"User with this Id {userModel.Id} not found");
        // }
        var comment = new Comment
        {
            Text = text,
            CreatedAt = DateTime.UtcNow,
            ParentCommentId = parentCommentId,
            UserId = userId
        };
        
        await commentRepository.AddAsync(comment, cancellationToken);
        return mapper.Map<CommentModel>(comment);
    }

    public async Task DeleteCommentAsync(int commentId, CancellationToken cancellationToken = default)
    {
        var commentDb = await commentRepository.GetByIdAsync(commentId, cancellationToken);
        if (commentDb == null)
            throw new CommentNotFoundException($"Comment with this Id {commentId} not found");
        
        await commentRepository.DeleteAsync(commentDb, cancellationToken);
    }

    public async Task<IEnumerable<CommentModel>> GetRepliesAsync(int commentId, CancellationToken cancellationToken = default)
    {
        var parentComment = await commentRepository.GetByIdAsync(commentId, cancellationToken);
        if (parentComment == null)
            throw new CommentNotFoundException($"Comment with ID {commentId} not found.");
        
        if (parentComment.ParentCommentId != null)
            throw new CommentIsNotMainException($"Comment with ID {commentId} is not a top-level comment.");
        
        var replies = await commentRepository.GetRepliesAsync(commentId, cancellationToken);
        
        return mapper.Map<IEnumerable<CommentModel>>(replies);
    }

    public async Task<IEnumerable<CommentModel>> GetCommentsTreeAsync(int commentId, CancellationToken cancellationToken = default)
    {
        var parentComment = await commentRepository.GetByIdAsync(commentId, cancellationToken);
        if (parentComment == null)
            throw new CommentNotFoundException($"Comment with ID {commentId} not found.");

        var tree = await commentRepository.GetCommentsTreeAsync(commentId, cancellationToken);
        return mapper.Map<IEnumerable<CommentModel>>(tree);
    }
}