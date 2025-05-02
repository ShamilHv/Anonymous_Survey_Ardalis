using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.Core.AdminAggregate;

public class Admin : EntityBase, IAggregateRoot
{
  private Admin() { }

  public string AdminName { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string PasswordHash { get; set; } = string.Empty;
  public int? SubjectId { get; set; }
  public int? DepartmentId { get; set; }
  public AdminRole Role { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiryTime { get; set; }

  // Navigation properties
  public virtual Subject? Subject { get; set; }
  public virtual Department? Department { get; set; }
  public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

  public static Admin CreateSubjectAdmin(string adminName, string email, int subjectId)
  {
    Guard.Against.NullOrEmpty(adminName, nameof(adminName));
    Guard.Against.NullOrEmpty(email, nameof(email));

    return new Admin
    {
      AdminName = adminName,
      Email = email,
      SubjectId = subjectId,
      DepartmentId = null,
      Role = AdminRole.SubjectAdmin,
      CreatedAt = DateTime.UtcNow
    };
  }

  public static Admin CreateDepartmentAdmin(string adminName, string email, int departmentId)
  {
    Guard.Against.NullOrEmpty(adminName, nameof(adminName));
    Guard.Against.NullOrEmpty(email, nameof(email));

    return new Admin
    {
      AdminName = adminName,
      Email = email,
      SubjectId = null,
      DepartmentId = departmentId,
      Role = AdminRole.DepartmentAdmin,
      CreatedAt = DateTime.UtcNow
    };
  }

  public static Admin CreateSuperAdmin(string adminName, string email)
  {
    Guard.Against.NullOrEmpty(adminName, nameof(adminName));
    Guard.Against.NullOrEmpty(email, nameof(email));

    return new Admin
    {
      AdminName = adminName,
      Email = email,
      SubjectId = null,
      DepartmentId = null,
      Role = AdminRole.SuperAdmin,
      CreatedAt = DateTime.UtcNow
    };
  }
}

public enum AdminRole
{
  SubjectAdmin,
  DepartmentAdmin,
  SuperAdmin
}
// using Ardalis.SharedKernel;
//
// namespace Anonymous_Survey_Ardalis.Core.AdminAggregate;
//
// public class Admin : EntityBase, IAggregateRoot
// {
//   public Admin(string adminName, string email, int? subjectId, AdminRole role = AdminRole.SubjectAdmin)
//   {
//     AdminName = adminName;
//     Email = email;
//     SubjectId = subjectId;
//     Role = role;
//     // Department ID will be determined based on the subject
//   }
//
//   public string AdminName { get; set; }
//   public string Email { get; set; }
//   public string PasswordHash { get; set; } = string.Empty;
//   public int? SubjectId { get; set; }
//   public int? DepartmentId { get; set; }  
//   public AdminRole Role { get; set; }
//   public DateTime CreatedAt { get; set; }
//   public string? RefreshToken { get; set; }
//   public DateTime? RefreshTokenExpiryTime { get; set; }
// }
//
// public enum AdminRole
// {
//   SubjectAdmin,
//   DepartmentAdmin,
//   SuperAdmin
// }
