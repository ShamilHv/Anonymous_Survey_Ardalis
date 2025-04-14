using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Config;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
  public void Configure(EntityTypeBuilder<Comment> builder)
  {
    builder.HasKey(c => c.Id);

    builder.Property(c => c.Id)
      .HasColumnName("comment_id");

    builder.Property(c => c.SubjectId)
      .HasColumnName("subject_id")
      .IsRequired();

    builder.Property(c => c.CommentText)
      .HasColumnName("comment_text")
      .HasColumnType("text")
      .IsRequired();

    builder.Property(c => c.CreatedAt)
      .HasColumnName("created_at")
      .HasDefaultValueSql("GETUTCDATE()");

    builder.Property(c => c.ParentCommentId)
      .HasColumnName("parent_comment_id");

    builder.Property(c => c.FileId)
      .HasColumnName("file_id");

    builder.Property(c => c.IsAdminComment)
      .HasColumnName("is_admin_comment")
      .HasDefaultValue(false);

    builder.Property(c => c.AdminId)
      .HasColumnName("admin_id");

    builder.HasOne(c => c.Subject)
      .WithMany()
      .HasForeignKey(c => c.SubjectId);

    builder.HasOne(c => c.ParentComment)
      .WithMany(c => c.ChildComments)
      .HasForeignKey(c => c.ParentCommentId);

    builder.HasOne(c => c.File)
      .WithMany()
      .HasForeignKey(c => c.FileId);

    builder.HasOne(c => c.Admin)
      .WithMany()
      .HasForeignKey(c => c.AdminId);
  }
}
