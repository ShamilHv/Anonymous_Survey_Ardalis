using Anonymous_Survey_Ardalis.UseCases.Comments;
using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Queries;

public class ListCommentQueryService : IListCommentQueryService
{
  public Task<IEnumerable<CommentDto>> ListAsync()
  {
    List<CommentDto> fakeComments =
    [
      new(1, 1, "klsdm", DateTime.UtcNow, null, "/", false),
      new(2, 2, "klsdm", DateTime.UtcNow, null, "/", false)
    ];

    return Task.FromResult(fakeComments.AsEnumerable());
  }
}
