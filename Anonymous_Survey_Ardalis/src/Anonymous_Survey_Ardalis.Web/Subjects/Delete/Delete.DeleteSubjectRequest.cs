namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class DeleteSubjectRequest
{
  public const string Route = "/Subjects/{SubjectId:int}";

  public int Subjectid { get; set; }

  public static string BuildRoute(int subjectId)
  {
    return Route.Replace("{SubjectId:int}", subjectId.ToString());
  }
}
