using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SPA.DAL.Entity;

namespace SPA.DAL.Configuration;

public class AttachmentConfiguration: IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.Comment)
            .WithMany(c => c.Attachments)
            .HasForeignKey(a => a.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
