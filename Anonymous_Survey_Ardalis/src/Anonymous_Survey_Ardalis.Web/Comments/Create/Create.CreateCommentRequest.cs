using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class CreateCommentRequest
{
  public const string Route = "/Comments";

  [Required] public int SubjectId { get; set; }

  [Required] public string CommentText { get; set; } = Guard.Against.NullOrEmpty(nameof(CommentText));

  public IFormFile? File { get; set; }
}
