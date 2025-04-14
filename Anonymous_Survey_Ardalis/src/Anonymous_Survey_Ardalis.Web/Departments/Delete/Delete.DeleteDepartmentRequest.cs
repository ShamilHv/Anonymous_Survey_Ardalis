namespace Anonymous_Survey_Ardalis.Web.Departments;

public class DeleteDepartmentRequest
{
  public const string Route = "/Departments/{DepartmentId:int}";

  public int DepartmentId { get; set; }

  public static string BuildRoute(int departmentId)
  {
    return Route.Replace("{DepartmentId:int}", departmentId.ToString());
  }
}
