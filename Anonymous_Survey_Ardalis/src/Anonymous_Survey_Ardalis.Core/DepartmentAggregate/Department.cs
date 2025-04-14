using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.Core.DepartmentAggregate;

public class Department(string departmentName) : EntityBase, IAggregateRoot
{
  public string DepartmentName { get; set; } = departmentName;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
