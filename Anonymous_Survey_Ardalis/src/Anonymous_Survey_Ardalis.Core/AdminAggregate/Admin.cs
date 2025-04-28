using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.Core.AdminAggregate;

// public class Admin : EntityBase, IAggregateRoot
// {
//   public Admin(string adminName, string email, int subjectId)
//   {
//     AdminName = adminName;
//     Email = email;
//     SubjectId = subjectId;
//   }
//
//   public string AdminName { get; set; }
//   public string Email { get; set; }
//   public string PasswordHash { get; set; } = string.Empty;
//   public int SubjectId { get; set; }
//   public DateTime CreatedAt { get; set; }
//   public string? RefreshToken { get; set; }
//   public DateTime? RefreshTokenExpiryTime { get; set; }
// }

public class Admin : EntityBase, IAggregateRoot
{
  public Admin(string adminName, string email, int subjectId, AdminRole role = AdminRole.SubjectAdmin)
  {
    AdminName = adminName;
    Email = email;
    SubjectId = subjectId;
    Role = role;
    // Department ID will be determined based on the subject
  }

  public string AdminName { get; set; }
  public string Email { get; set; }
  public string PasswordHash { get; set; } = string.Empty;
  public int SubjectId { get; set; }
  public int? DepartmentId { get; set; }  
  public AdminRole Role { get; set; }
  public DateTime CreatedAt { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiryTime { get; set; }
}

public enum AdminRole
{
  SubjectAdmin,
  DepartmentAdmin,
  SuperAdmin
}
