namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class GetSubjectByIdRequest
{
  public const string Route = "/Subjects/{SubjectId:int}";

  public int SubjectId { get; set; }

  public static string BuildRoute(int subjectId)
  {
    return Route.Replace("{SubjectId:int}", subjectId.ToString());
  }
}
