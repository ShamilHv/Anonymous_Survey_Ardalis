using Anonymous_Survey_Ardalis.UseCases.Comments;
using Ardalis.GuardClauses;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects;

public class SubjectWithCommentsDto
{
  public int SubjectId { get; set; }
  public string SubjectName { get; set; } = Guard.Against.NullOrEmpty(nameof(SubjectName));
  public int DepartmentId { get; set; }
  public DateTime CreatedAt { get; set; }
  public List<CommentDto> comments { get; set; } = new();
}
