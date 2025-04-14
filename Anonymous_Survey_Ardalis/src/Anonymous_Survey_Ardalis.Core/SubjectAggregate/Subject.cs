using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.Core.SubjectAggregate;

public class Subject(string subjectName, int departmentId) : EntityBase, IAggregateRoot
{
  public string SubjectName { get; set; } = subjectName;

  public int DepartmentId { get; set; } = departmentId;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public virtual Department? Department { get; set; }

  public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();
  public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
