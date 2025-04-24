namespace Anonymous_Survey_Ardalis.Web.Comments.Create.CreateAdminComment;

public class CreateAdminCommentResponse(int parentCommentId, string commentText)
{
  public int CommentId { get; set; }
  public int SubjectId { get; set; } 
  public string CommentText { get; set; } = commentText;
  public int? ParentCommentId { get; set; } = parentCommentId;
  public bool IsAdminComment { get; set; } = true;
}
