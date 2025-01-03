using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SPA.DAL.Entity;

namespace SPA.DAL.Configuration;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.HasOne(u => u.AuthorizationInfo)
            .WithOne(a => a.User)
            .HasForeignKey<AuthorizationInfo>(a => a.UserId);

        builder.HasMany(u => u.Comments) 
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}