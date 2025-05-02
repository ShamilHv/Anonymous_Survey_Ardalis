using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Config;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
  public void Configure(EntityTypeBuilder<Admin> builder)
  {
    builder.ToTable("Admins");

    builder.HasKey(a => a.Id);

    // Basic properties
    builder.Property(a => a.Email)
      .IsRequired()
      .HasMaxLength(100);

    builder.Property(a => a.AdminName)
      .IsRequired()
      .HasMaxLength(50);

    builder.Property(a => a.PasswordHash)
      .IsRequired()
      .HasMaxLength(128);

    builder.Property(a => a.CreatedAt)
      .HasDefaultValueSql("GETUTCDATE()");

    builder.HasIndex(a => a.Email)
      .IsUnique();

    // Subject relationship
    builder.HasOne(a => a.Subject)
      .WithMany(s => s.Admins)
      .HasForeignKey(a => a.SubjectId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.NoAction);

    // IMPORTANT: Define the Department relationship EXPLICITLY
    builder.HasOne(a => a.Department)
      .WithMany() // If Department has a collection of Admins, specify it here
      .HasForeignKey(a => a.DepartmentId) // Use the EXACT property name
      .IsRequired(false)
      .OnDelete(DeleteBehavior.NoAction);

    // Comments relationship
    builder.HasMany(a => a.Comments)
      .WithOne(c => c.Admin)
      .HasForeignKey(c => c.AdminId)
      .IsRequired(false);
  }
  // public void Configure(EntityTypeBuilder<Admin> builder)
  // {
  //   builder.ToTable("Admins");
  //
  //   builder.HasKey(a => a.Id);
  //
  //   builder.Property(a => a.Email)
  //     .IsRequired()
  //     .HasMaxLength(100);
  //
  //   builder.Property(a => a.AdminName)
  //     .IsRequired()
  //     .HasMaxLength(50);
  //
  //   builder.Property(a => a.PasswordHash)
  //     .IsRequired()
  //     .HasMaxLength(128);
  //
  //   builder.Property(a => a.CreatedAt)
  //     .HasDefaultValueSql("GETUTCDATE()");
  //
  //   builder.HasIndex(a => a.Email)
  //     .IsUnique();
  //
  //   // Configure relationships based on role types
  //   
  //   // Subject relationship - only when SubjectId is not null
  //   builder.HasOne(a => a.Subject)
  //     .WithMany(s => s.Admins)
  //     .HasForeignKey(a => a.SubjectId)
  //     .IsRequired(false) // Make it optional
  //     .OnDelete(DeleteBehavior.NoAction);
  //   
  //   // Department relationship - only when DepartmentId is not null
  //   builder.HasOne(a => a.Department)
  //     .WithMany()
  //     .HasForeignKey(a => a.DepartmentId)
  //     .IsRequired(false) // Make it optional
  //     .OnDelete(DeleteBehavior.NoAction);
  //
  //   // Comments relationship
  //   builder.HasMany(a => a.Comments)
  //     .WithOne(c => c.Admin)
  //     .HasForeignKey(c => c.AdminId)
  //     .IsRequired(false); // AdminId in Comments is optional
  // }
}
// using Anonymous_Survey_Ardalis.Core.AdminAggregate;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Config;
//
// public class AdminConfiguration : IEntityTypeConfiguration<Admin>
// {
//   public void Configure(EntityTypeBuilder<Admin> builder)
//   {
//     builder.ToTable("Admins");
//
//     builder.HasKey(a => a.Id);
//
//     builder.Property(a => a.Email)
//       .IsRequired()
//       .HasMaxLength(100);
//
//     builder.Property(a => a.AdminName)
//       .IsRequired()
//       .HasMaxLength(50);
//
//     builder.Property(a => a.PasswordHash)
//       .IsRequired()
//       .HasMaxLength(128);
//
//     builder.HasIndex(a => a.Email)
//       .IsUnique();
//
//     // Relationship with Comments (if applicable)
//     // builder.HasMany(a => a.Comments)
//     //     .WithOne(c => c.Admin)
//     //     .HasForeignKey(c => c.AdminId);
//   }
// }
