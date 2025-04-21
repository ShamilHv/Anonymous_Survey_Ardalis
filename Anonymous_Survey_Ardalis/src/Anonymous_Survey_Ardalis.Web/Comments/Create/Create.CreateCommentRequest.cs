using Ardalis.GuardClauses;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class CreateCommentRequest
{
  public const string Route = "/Comments";

  public int SubjectId { get; set; }

  public string CommentText { get; set; } = Guard.Against.NullOrEmpty(nameof(CommentText));

  public IFormFile? File { get; set; }
}
