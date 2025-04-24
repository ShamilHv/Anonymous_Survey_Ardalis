namespace Anonymous_Survey_Ardalis.Web.Departments.Get.GetWithSubjects;

public class GetDepartmentWithSubjectsRequest
{
  public const string Route = "/Departments/{DepartmentId:int}/Subjects";

  public int DepartmentId { get; set; }

  public static string BuildRoute(int departmentId)
  {
    return Route.Replace("{DepartmentId:int}", departmentId.ToString());
  }
}
