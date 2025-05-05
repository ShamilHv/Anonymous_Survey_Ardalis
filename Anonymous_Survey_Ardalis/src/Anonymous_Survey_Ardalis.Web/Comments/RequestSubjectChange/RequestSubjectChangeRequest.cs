namespace Anonymous_Survey_Ardalis.Web.Comments.RequestSubjectChange;

public class RequestSubjectChangeRequest
{
  public const string Route = "/Comments/RequestSubjectChange";

  public int CommentId { get; set; }
  public int SuggestedSubjectId { get; set; }
  public string? Message { get; set; }
}
