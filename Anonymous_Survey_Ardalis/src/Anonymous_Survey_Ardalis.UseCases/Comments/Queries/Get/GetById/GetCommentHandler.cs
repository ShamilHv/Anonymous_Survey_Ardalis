using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get;

public class GetCommentHandler(IReadRepository<Comment> repository) : IQueryHandler<GetCommentQuery, Result<CommentDto>>
{
  public async Task<Result<CommentDto>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
  {
    var spec = new CommentByIdSpec(request.CommentId);
    var comment = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (comment == null)
    {
      return Result.NotFound();
    }

    return new CommentDto(comment.Id, comment.SubjectId, comment.CommentText, comment.CreatedAt,
      comment.ParentCommentId, comment.File?.FileId, comment.IsAdminComment);
  }
}
