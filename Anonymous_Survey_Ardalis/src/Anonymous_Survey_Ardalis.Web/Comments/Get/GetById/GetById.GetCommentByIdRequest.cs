namespace Anonymous_Survey_Ardalis.Web.Comments;

public class GetCommentByIdRequest
{
  public const string Route = "/Comments/{CommentId:int}";

  public int CommentId { get; set; }

  public static string BuildRoute(int commentId)
  {
    return Route.Replace("{CommentId:int}", commentId.ToString());
  }
}
