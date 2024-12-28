using Microsoft.EntityFrameworkCore;
using SPA.DAL.Entity;
using SPA.DAL.Repositories.Interfaces;

namespace SPA.DAL.Repositories;

public class CommentRepository(SPADbContext spaDbContext) : ICommentRepository
{
    private readonly SPADbContext _spaDbContext = spaDbContext;
    
    private async Task LoadRepliesAsync(Comment comment, CancellationToken cancellationToken)
    {
        var replies = await _spaDbContext.Comments
            .Include(c => c.User)
            .Include(c => c.Replies)
            .ThenInclude(r => r.User)
            .Where(c => c.ParentCommentId == comment.Id)
            .ToListAsync(cancellationToken);

        comment.Replies = replies;
        
        foreach (var reply in replies)
        {
            await LoadRepliesAsync(reply, cancellationToken);
        }
    }
    public IQueryable<Comment> GetAll()
    {
        return _spaDbContext.Comments
            .Include(i => i.User)
            .Include(i => i.Replies)
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
    public async Task<IEnumerable<Comment>> GetRepliesAsync(int commentId, CancellationToken cancellationToken = default)
    {
        return await _spaDbContext.Comments
            .Include(c => c.Replies)
            .Where(c => c.ParentCommentId == commentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Comment?>> GetCommentsTreeAsync(int commentId, CancellationToken cancellationToken = default)
    {
        var comment = await _spaDbContext.Comments
            .Include(c => c.User)
            .Include(c => c.Replies)
            .ThenInclude(r => r.User)
            .Include(c => c.Attachments)
            .Where(c => c.Id == commentId)
            .FirstOrDefaultAsync(cancellationToken);
        
        await LoadRepliesAsync(comment, cancellationToken);

        return new List<Comment?> { comment };
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