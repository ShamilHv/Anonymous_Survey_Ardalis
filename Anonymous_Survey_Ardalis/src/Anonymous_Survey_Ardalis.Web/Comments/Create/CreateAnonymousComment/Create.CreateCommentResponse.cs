namespace Anonymous_Survey_Ardalis.Web.Comments.Create.CreateAnonymousComment;

public class CreateCommentResponse(int subjectId, string commentText)
{
  public int CommentId { get; set; }
  public int SubjectId { get; set; } = subjectId;
  public string CommentText { get; set; } = commentText;
  public int? ParentCommentId { get; set; }
  public bool HasFile { get; set; }
  public string? FilePath { get; set; }
  public bool IsAdminComment { get; set; }
}
