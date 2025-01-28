using Microsoft.EntityFrameworkCore;
using SPA.DAL.Entity;
using SPA.DAL.Repositories.Interfaces;

namespace SPA.DAL.Repositories;

public class CommentRepository(SPADbContext spaDbContext) : ICommentRepository
{
    private readonly SPADbContext _spaDbContext = spaDbContext;
    public IQueryable<Comment> GetAll()
    {
        return _spaDbContext.Comments
            .Include(i => i.User)
            .Include(i => i.Attachments)
            .AsQueryable();
    }

    public async Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _spaDbContext.Comments
            .Include(i => i.User)
            .Include(i => i.Replies)
            .Include(i => i.Attachments)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken: cancellationToken);
    }
    // public IQueryable<Comment> GetRepliesAsync(int commentId)
    // {
    //     var query=  _spaDbContext.Comments
    //         .Include(c => c.Replies)
    //         .Where(c => c.ParentCommentId == commentId);
    //     return query;
    // }
    public IQueryable<Comment> GetAllParentCommentsAsync()
    {
        var query=  _spaDbContext.Comments
            .Where(c => c.ParentCommentId == null);
        return query;
    }
    public async Task<IEnumerable<Comment>> GetTreeAsync(int commentId, CancellationToken cancellationToken = default)
    {
        var comment = await _spaDbContext.Comments
            .Include(c => c.User)
            .Include(c => c.Attachments)
            .Where(c => c.Id == commentId || c.ParentCommentId == commentId)
            .ToListAsync(cancellationToken);

        return comment;
    }

    public async Task<IEnumerable<Comment>> GetAllTreesAsync(CancellationToken cancellationToken = default)
    {
        var comments = await _spaDbContext.Comments
            .Include(c => c.User)
            .Include(c => c.Attachments)
            .ToListAsync(cancellationToken);
        
        var sortedComments = SortCommentsByHierarchy(comments);

        return sortedComments;
    }


    private List<Comment> SortCommentsByHierarchy(List<Comment> comments)
    {
        var sortedComments = new List<Comment>();
        
        void AddWithReplies(Comment comment)
        {
            sortedComments.Add(comment);
            
            var replies = comments.Where(c => c.ParentCommentId == comment.Id).OrderBy(c => c.CreatedAt);
            foreach (var reply in replies)
            {
                AddWithReplies(reply);
            }
        }
        
        var rootComments = comments.Where(c => c.ParentCommentId == null).OrderBy(c => c.CreatedAt);
        foreach (var rootComment in rootComments)
        {
            AddWithReplies(rootComment);
        }

        return sortedComments;
    }


    public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        await _spaDbContext.Comments.AddAsync(comment, cancellationToken);
        await _spaDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _spaDbContext.Comments.Remove(comment);
        await _spaDbContext.SaveChangesAsync(cancellationToken);
    }
}