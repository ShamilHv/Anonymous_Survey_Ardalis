namespace Anonymous_Survey_Ardalis.Web.Comments.Get.GetWithReplies;

public class GetCommentWithRepliesRequest
{
  public const string Route = "/Comments/{CommentId:int}/Replies";

  public int CommentId { get; set; }

  public static string BuildRoute(int commentId)
  {
    return Route.Replace("{CommentId:int}", commentId.ToString());
  }
}
