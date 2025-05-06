namespace Anonymous_Survey_Ardalis.Web.Comments.ReportInappropriateComment;

public class ReportInappropriateCommentRequest
{
  public static string Route => "/Comments/ReportInappropriate";
    
  public int CommentId { get; set; }
  public string? Message { get; set; }
}
