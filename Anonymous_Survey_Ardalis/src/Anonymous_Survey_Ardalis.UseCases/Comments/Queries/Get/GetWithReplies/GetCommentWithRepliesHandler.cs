using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get.GetWithReplies;

public class GetCommentWithRepliesHandler(IReadRepository<Comment> repository)
  : IRequestHandler<GetCommentWithRepliesQuery, Result<CommentWithRepliesDto>>
{
  public async Task<Result<CommentWithRepliesDto>> Handle(GetCommentWithRepliesQuery request,
    CancellationToken cancellationToken)
  {
    var specForComment = new CommentByGuidSpec(request.CommentIdentifier);
    var comment = await repository.FirstOrDefaultAsync(specForComment, cancellationToken);
    var spec = new CommentWithRepliesSpec(request.CommentIdentifier);
    var replies = await repository.ListAsync(spec, cancellationToken);

    if (replies == null || comment == null || !replies.Any())
    {
      return Result.NotFound();
    }

    var commentDtos = replies.Select(MapToCommentDto).ToList();

    return new CommentWithRepliesDto(comment.Id, comment.SubjectId, comment.CommentText,
      comment.CreatedAt, comment.ParentCommentId, comment.FileId, comment.IsAdminComment, commentDtos);
  }

  private CommentDto MapToCommentDto(Comment comment)
  {
    return new CommentDto(comment.Id, comment.SubjectId, comment.CommentText, comment.CreatedAt,
      comment.ParentCommentId, comment.FileId, comment.IsAdminComment);
  }
}
