using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;

public class ListCommentsHandler(IListCommentQueryService _query)
  : IQueryHandler<ListCommentsQuery, Result<IEnumerable<CommentDto>>>
{
  public async Task<Result<IEnumerable<CommentDto>>> Handle(ListCommentsQuery request,
    CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();
    return Result.Success(result);
  }
}
