using Microsoft.EntityFrameworkCore;
using SPA.DAL.Entity;

namespace SPA.DAL;

public class SPADbContext : DbContext
{
    public SPADbContext(DbContextOptions<SPADbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<AuthorizationInfo> AuthorizationInfos { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SPADbContext).Assembly);
    }
}