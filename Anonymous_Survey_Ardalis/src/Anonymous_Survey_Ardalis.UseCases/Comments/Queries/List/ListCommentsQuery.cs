using Anonymous_Survey_Ardalis.UseCases.Common;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;

public record ListCommentsQuery : IQuery<Result<PagedResponse<CommentDto>>>
{
  public int PageNumber { get; init; } = 1;
  public int PageSize { get; init; } = 10;
  public int? SubjectId { get; init; }
  public int? DepartmentId { get; init; }
}
