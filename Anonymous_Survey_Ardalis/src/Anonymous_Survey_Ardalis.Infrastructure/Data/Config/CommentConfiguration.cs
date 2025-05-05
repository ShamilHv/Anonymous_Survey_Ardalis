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

    // Fix this relationship - make it match the relationship in AdminConfiguration
    builder.HasOne(c => c.Admin)
      .WithMany(a => a.Comments) // Specify the property name in Admin class
      .HasForeignKey(c => c.AdminId)
      .IsRequired(false); // Match the IsRequired setting
  }
}
