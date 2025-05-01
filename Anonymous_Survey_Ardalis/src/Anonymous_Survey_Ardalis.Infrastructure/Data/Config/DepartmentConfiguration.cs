using Anonymous_Survey_Ardalis.Core.AdminAggregate;
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

    builder.Property(d => d.CreatedAt)
      .HasDefaultValueSql("GETUTCDATE()");

    // Relationship with Subjects
    builder.HasMany(d => d.Subjects)
      .WithOne(s => s.Department)
      .HasForeignKey(s => s.DepartmentId)
      .OnDelete(DeleteBehavior.NoAction);
      
    // IMPORTANT: Add this only if you have a collection of Admins in Department class
    // If you don't have this collection, REMOVE this configuration
    builder.HasMany(d => d.Admins) 
      .WithOne(a => a.Department)
      .HasForeignKey(a => a.DepartmentId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.NoAction);
  }
}
// using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Config;
//
// public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
// {
//   public void Configure(EntityTypeBuilder<Department> builder)
//   {
//     builder.ToTable("Departments");
//
//     builder.HasKey(d => d.Id);
//
//     builder.Property(d => d.DepartmentName)
//       .IsRequired()
//       .HasMaxLength(100);
//
//     builder.HasMany(d => d.Subjects)
//       .WithOne(s => s.Department)
//       .HasForeignKey(s => s.DepartmentId)
//       .OnDelete(DeleteBehavior.Cascade);
//   }
// }
