using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = Anonymous_Survey_Ardalis.Core.CommentAggregate.File;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Config;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
  public void Configure(EntityTypeBuilder<File> builder)
  {
    builder.ToTable("Files");

    builder.HasKey(f => f.FileId);

    builder.Property(f => f.FilePath)
      .IsRequired()
      .HasMaxLength(1000);

    builder.Property(f => f.UploadedAt)
      .HasDefaultValueSql("GETUTCDATE()");
  }
}
