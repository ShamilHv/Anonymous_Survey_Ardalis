namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class CreateSubjectResponse(int departmentId, string subjectName)
{
  public int SubjectId { get; set; }
  public string SubjectName { get; set; } = subjectName;
  public int DepartmentId { get; set; } = departmentId;
  public DateTime CreatedAt { get; set; }
}
