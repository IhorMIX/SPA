using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPA.BLL.Exceptions;
using SPA.BLL.Extensions;
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

    public async Task<CommentModel> AddCommentAsync(CommentModel commentModel,int userId, CancellationToken cancellationToken = default)
    {
        var comment = new Comment
        {
            Text = commentModel.Text,
            CreatedAt = DateTime.UtcNow,
            ParentCommentId = commentModel.ParentCommentId,
            UserId = userId,
            Attachments = commentModel.Attachments.Select(a => new Attachment
            {
                FileURL = a.FileURL
            }).ToList()
        };
        await commentRepository.AddAsync(comment, cancellationToken);
        
        foreach (var attachment in comment.Attachments)
        {
            attachment.CommentId = comment.Id;
        } 
        return mapper.Map<CommentModel>(comment);
    }

    public async Task DeleteCommentAsync(int commentId, CancellationToken cancellationToken = default)
    {
        var commentDb = await commentRepository.GetByIdAsync(commentId, cancellationToken);
        if (commentDb == null)
            throw new CommentNotFoundException($"Comment with this Id {commentId} not found");
        
        await commentRepository.DeleteAsync(commentDb, cancellationToken);
    }

    public async Task<PaginationResultModel<CommentModel>> GetAllParentCommentsAsync(
        PaginationModel pagination,
        CancellationToken cancellationToken = default)
    {
        var parentCommentsQuery = commentRepository.GetAllParentCommentsAsync();

        var parentComments = await parentCommentsQuery
            .Pagination(pagination.CurrentPage, pagination.PageSize)
            .ToListAsync(cancellationToken);

        var commentModels = mapper.Map<IEnumerable<CommentModel>>(parentComments);
        
        var paginationResult = new PaginationResultModel<CommentModel>
        {
            Data = commentModels,
            CurrentPage = pagination.CurrentPage,
            PageSize = pagination.PageSize,
            TotalItems = await parentCommentsQuery.CountAsync(cancellationToken),
        };

        return paginationResult;
    }
    

    public async Task<IEnumerable<CommentModel>> GetTreeByCommentIdAsync(int commentId, CancellationToken cancellationToken = default)
    {
        var parentComment = await commentRepository.GetByIdAsync(commentId, cancellationToken);
        if (parentComment == null)
            throw new CommentNotFoundException($"Comment with ID {commentId} not found.");

        var tree = await commentRepository.GetTreeAsync(commentId, cancellationToken);
        return mapper.Map<IEnumerable<CommentModel>>(tree);
    }
    public async Task<IEnumerable<CommentModel>> GetAllCommentTreesAsync(
        PaginationModel pagination,
        CancellationToken cancellationToken = default)
    {
        var trees = await commentRepository.GetAllTreesAsync(cancellationToken);
        
        // var paginatedTrees = trees
        //     .Skip((pagination.CurrentPage - 1) * pagination.PageSize)
        //     .Take(pagination.PageSize)
        //     .ToList();
        
        var totalItems = trees.Count();
        
        var mappedTrees = mapper.Map<IEnumerable<CommentModel>>(trees);
        
        // var paginationResult = new PaginationResultModel<CommentModel>
        // {
        //     Data = mappedTrees,
        //     CurrentPage = pagination.CurrentPage,
        //     PageSize = pagination.PageSize,
        //     TotalItems = totalItems,
        // };

        return mappedTrees;
    }


}