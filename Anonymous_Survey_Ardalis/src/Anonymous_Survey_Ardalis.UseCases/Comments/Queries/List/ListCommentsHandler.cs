using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.UseCases.Departments;
using Anonymous_Survey_Ardalis.UseCases.Departments.Queries.List;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;

public class ListCommentsHandler(IReadRepository<Comment> commentRepository)
  : IQueryHandler<ListCommentsQuery, Result<IEnumerable<CommentDto>>>
{
  public async Task<Result<IEnumerable<CommentDto>>> Handle(ListCommentsQuery request,
    CancellationToken cancellationToken)
  {
    var comments = await commentRepository.ListAsync(cancellationToken);
    var commmentDtos = comments.Select(c =>
      new CommentDto(c.Id, c.SubjectId, c.CommentText, c.CreatedAt, c.ParentCommentId, c.FileId, c.IsAdminComment));
    return Result.Success(commmentDtos);
  }
}
