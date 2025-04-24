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

    builder.Property(a => a.Email)
      .IsRequired()
      .HasMaxLength(100);

    builder.Property(a => a.AdminName)
      .IsRequired()
      .HasMaxLength(50);

    builder.Property(a => a.PasswordHash)
      .IsRequired()
      .HasMaxLength(128);

    builder.HasIndex(a => a.Email)
      .IsUnique();

    // Relationship with Comments (if applicable)
    // builder.HasMany(a => a.Comments)
    //     .WithOne(c => c.Admin)
    //     .HasForeignKey(c => c.AdminId);
  }
}
