namespace Anonymous_Survey_Ardalis.Web.Subjects.Get.GetWithComments;

public class GetSubjectWithCommentsRequest
{
  public const string Route = "/Subjects/{SubjectId:int}/Comments";

  public int SubjectId { get; set; }

  public static string BuildRoute(int subjectId)
  {
    return Route.Replace("{DepartmentId:int}", subjectId.ToString());
  }
}
