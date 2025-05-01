using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get.GetByGuid;

public class GetCommentByGuidHandler(IReadRepository<Comment> repository) : IQueryHandler<GetCommentByGuidQuery, Result<CommentDto>>
{
  public async Task<Result<CommentDto>> Handle(GetCommentByGuidQuery request, CancellationToken cancellationToken)
  {
    var spec = new CommentByGuidSpec(request.CommentIdentifier);
    var comment = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (comment == null)
    {
      return Result.NotFound();
    }

    return new CommentDto(comment.Id, comment.SubjectId, comment.CommentText, comment.CreatedAt,
      comment.ParentCommentId, comment.File?.FileId, comment.IsAdminComment);
  }
}
