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
    var spec = new CommentWithRepliesSpec(request.CommentId);
    var replies = await repository.ListAsync(spec, cancellationToken);

    if (replies == null || !replies.Any())
    {
      return Result.NotFound();
    }

    var commentDtos = replies.Select(MapToCommentDto).ToList();

    return new CommentWithRepliesDto(commentDtos);
  }

  private CommentDto MapToCommentDto(Comment comment)
  {
    return new CommentDto(comment.Id, comment.SubjectId, comment.CommentText, comment.CreatedAt,
      comment.ParentCommentId, comment.FileId, comment.IsAdminComment);
  }
}
//
// public class GetCommentWithRepliesHandler(IReadRepository<Comment> repository)
//   : IRequestHandler<GetCommentWithRepliesQuery, Result<CommentWithRepliesDto>>
// {
//   public async Task<Result<CommentWithRepliesDto>> Handle(GetCommentWithRepliesQuery request, CancellationToken cancellationToken)
//   {
//     var spec = new CommentWithRepliesSpec(request.CommentId);
//     var comment = await repository.FirstOrDefaultAsync(spec, cancellationToken);
//     if (comment == null)
//     {
//       return Result.NotFound();
//     }
//     
//     var commentDtos = comment.ChildComments.Select(MapToCommentDto).ToList();
//     
//     return new CommentWithRepliesDto(commentDtos);
//   }
//   
//   private CommentDto MapToCommentDto(Comment comment)
//   {
//     return new CommentDto(comment.Id, comment.SubjectId, comment.CommentText, comment.CreatedAt,
//       comment.ParentCommentId, comment.FileId, comment.IsAdminComment);
//
//   }
// }
