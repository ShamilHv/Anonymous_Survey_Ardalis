using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Config;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
  public void Configure(EntityTypeBuilder<Comment> builder)
  {
    builder.ToTable("Comments");

    builder.HasKey(c => c.Id);

    builder.Property(c => c.SubjectId)
      .IsRequired();

    builder.Property(c => c.CommentText)
      .HasColumnType("text")
      .IsRequired();

    builder.Property(c => c.CreatedAt)
      .HasDefaultValueSql("GETUTCDATE()");

    // Add the new property with default value
    builder.Property(c => c.IsAppropriate)
      .HasDefaultValue(true);

    builder.HasOne(c => c.Subject)
      .WithMany()
      .HasForeignKey(c => c.SubjectId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(c => c.ParentComment)
      .WithMany(c => c.ChildComments)
      .HasForeignKey(c => c.ParentCommentId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(c => c.File)
      .WithMany()
      .HasForeignKey(c => c.FileId)
      .OnDelete(DeleteBehavior.SetNull);

    builder.HasOne(c => c.Admin)
      .WithMany(a => a.Comments)
      .HasForeignKey(c => c.AdminId)
      .IsRequired(false);
  }
}
