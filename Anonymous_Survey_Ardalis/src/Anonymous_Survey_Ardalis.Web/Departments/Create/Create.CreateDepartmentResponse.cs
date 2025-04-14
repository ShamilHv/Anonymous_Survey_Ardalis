namespace Anonymous_Survey_Ardalis.Web.Departments;

public class CreateDepartmentResponse
{
  public int DepartmentId { get; set; }
  public string DepartmentName { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
}
