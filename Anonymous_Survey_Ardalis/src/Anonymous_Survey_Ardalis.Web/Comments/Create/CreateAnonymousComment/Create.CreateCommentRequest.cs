using Microsoft.AspNetCore.Mvc;

public class CreateCommentRequest
{
  public const string Route = "/Comments";

  public int SubjectId { get; set; }

  public string CommentText { get; set; } = string.Empty;

  public IFormFile? File { get; set; }
}
