using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.Core.AdminAggregate;

public class Admin(string adminName, string email, int subjectId) : EntityBase, IAggregateRoot
{
  public string AdminName { get; set; } = adminName;

  public string Email { get; set; } = email;
  public string PasswordHash { get; set; } = string.Empty;

  public int SubjectId { get; set; } = subjectId;

  public DateTime CreatedAt { get; set; }

  public string? RefreshToken { get; set; }

  public DateTime? RefreshTokenExpiryTime { get; set; }
}
