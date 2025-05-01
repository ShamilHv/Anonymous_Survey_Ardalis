namespace Anonymous_Survey_Ardalis.Web.Comments.Get.GetWithReplies;

public class GetCommentWithRepliesRequest
{
  public const string Route = "/Comments/{CommentGuid}/Replies";

  public Guid CommentGuid { get; set; }

  public static string BuildRoute(Guid commentIdentifier)
  {
    return Route.Replace("{CommentGuid}", commentIdentifier.ToString());
  }
}
