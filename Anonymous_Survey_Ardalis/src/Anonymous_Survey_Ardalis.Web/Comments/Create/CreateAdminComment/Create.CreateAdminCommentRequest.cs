using Ardalis.GuardClauses;

namespace Anonymous_Survey_Ardalis.Web.Comments.Create.CreateAdminComment;

public class CreateAdminCommentRequest
{
  public const string Route = "/Comments/Admin";

  public int ParentCommentId { get; set; }

  public string CommentText { get; set; } = Guard.Against.NullOrEmpty(nameof(CommentText));
}
