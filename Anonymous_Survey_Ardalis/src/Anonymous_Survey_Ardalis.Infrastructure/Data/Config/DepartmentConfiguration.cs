using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Config;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
  public void Configure(EntityTypeBuilder<Department> builder)
  {
    builder.ToTable("Departments");

    builder.HasKey(d => d.Id);

    builder.Property(d => d.DepartmentName)
      .IsRequired()
      .HasMaxLength(100);

    builder.HasMany(d => d.Subjects)
      .WithOne(s => s.Department)
      .HasForeignKey(s => s.DepartmentId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
