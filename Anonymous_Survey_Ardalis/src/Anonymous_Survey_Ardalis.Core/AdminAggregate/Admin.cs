using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.Core.AdminAggregate;

public class Admin : EntityBase, IAggregateRoot
{
  public string AdminName { get; set; } = Guard.Against.NullOrEmpty(nameof(AdminName));

  public string Email { get; set; } = Guard.Against.NullOrEmpty(nameof(Email));

  public string PasswordHash { get; set; } = string.Empty;

  public int SubjectId { get; set; }

  public DateTime CreatedAt { get; set; }

  public string? RefreshToken { get; set; }

  public DateTime? RefreshTokenExpiryTime { get; set; }
}
