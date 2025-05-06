namespace Anonymous_Survey_Ardalis.Web.Comments.UpdateSubject;

public class UpdateCommentSubjectRequest
{
  public const string Route = "/api/comments/{CommentId}/subject";

  public int CommentId { get; set; }
  public int NewSubjectId { get; set; }
}
