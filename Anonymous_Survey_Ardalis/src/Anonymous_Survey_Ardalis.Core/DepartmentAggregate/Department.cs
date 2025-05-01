using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.Core.DepartmentAggregate;

public class Department(string departmentName) : EntityBase, IAggregateRoot
{
  public string DepartmentName { get; set; } = departmentName;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
  
  public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

}
