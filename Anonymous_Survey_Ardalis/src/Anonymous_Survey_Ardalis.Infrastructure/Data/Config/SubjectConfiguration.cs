using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Config;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
  public void Configure(EntityTypeBuilder<Subject> builder)
  {
    builder.ToTable("Subjects");

    builder.HasKey(s => s.Id);

    builder.Property(s => s.SubjectName)
      .IsRequired()
      .HasMaxLength(100);

    builder.Property(s => s.DepartmentId)
      .IsRequired();


    builder.HasMany(s => s.Comments)
      .WithOne(c => c.Subject)
      .HasForeignKey(c => c.SubjectId)
      .OnDelete(DeleteBehavior.NoAction);
  }
}
