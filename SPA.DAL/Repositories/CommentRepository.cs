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

    public async Task<IEnumerable<Comment>> GetCommentsTreeAsync(CancellationToken cancellationToken = default)
    {
        return await _spaDbContext.Comments
            .Include(c => c.User)
            .Include(c => c.Replies)
            .Include(c => c.Attachments)
            .Where(c => c.ParentCommentId == null)
            .ToListAsync(cancellationToken);
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