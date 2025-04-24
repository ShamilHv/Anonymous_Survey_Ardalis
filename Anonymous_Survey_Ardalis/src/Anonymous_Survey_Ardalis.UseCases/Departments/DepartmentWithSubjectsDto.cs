using Anonymous_Survey_Ardalis.UseCases.Subjects;
using Ardalis.GuardClauses;

namespace Anonymous_Survey_Ardalis.UseCases.Departments;

public class DepartmentWithSubjectsDto
{
  public int DepartmentId { get; set; }
  public string DepartmentName { get; set; } = Guard.Against.NullOrEmpty(nameof(DepartmentName));
  public DateTime CreatedAt { get; set; }
  public List<SubjectDto> subjects { get; set; } = new();
}
