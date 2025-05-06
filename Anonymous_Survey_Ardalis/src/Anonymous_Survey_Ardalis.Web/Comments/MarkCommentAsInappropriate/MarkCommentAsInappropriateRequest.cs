namespace Anonymous_Survey_Ardalis.Web.Comments.MarkCommentAsInappropriate;

public class MarkCommentAsInappropriateRequest
{
  public static string Route => "/Comments/MarkAsInappropriate";
    
  public int CommentId { get; set; }
}
