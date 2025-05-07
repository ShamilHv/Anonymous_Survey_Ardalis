using Microsoft.AspNetCore.Mvc;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class GetCommentByGuidRequest
{
  public const string Route = "/Comments/{CommentGuid}";

  [FromRoute] public Guid CommentGuid { get; set; }

  public static string BuildRoute(Guid commentIdentifier)
  {
    return Route.Replace("{CommentGuid}", commentIdentifier.ToString());
  }
}
