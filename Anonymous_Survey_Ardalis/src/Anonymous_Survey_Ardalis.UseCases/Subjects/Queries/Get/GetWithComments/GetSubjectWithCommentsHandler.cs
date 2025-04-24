using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate.Specifications;
using Anonymous_Survey_Ardalis.UseCases.Comments;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.Get.GetWithComments;

public class GetSubjectWithCommentsHandler(IReadRepository<Subject> repository)
  : IRequestHandler<GetSubjectWithCommentsQuery, Result<SubjectWithCommentsDto>>
{
  public async Task<Result<SubjectWithCommentsDto>> Handle(GetSubjectWithCommentsQuery request,
    CancellationToken cancellationToken)
  {
    var spec = new SubjectWithCommentsSpec(request.Id);
    var subject = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (subject == null)
    {
      return Result.NotFound();
    }

    var commentDtos = subject.Comments.Select(MapToCommentDto).ToList();
    return new SubjectWithCommentsDto
    {
      SubjectId = subject.Id,
      SubjectName = subject.SubjectName,
      CreatedAt = subject.CreatedAt,
      comments = commentDtos,
      DepartmentId = subject.DepartmentId
    };
  }

  private CommentDto MapToCommentDto(Comment comment)
  {
    return new CommentDto(comment.Id, comment.SubjectId, comment.CommentText, comment.CreatedAt,
      comment.ParentCommentId, comment.FileId, comment.IsAdminComment);
  }
}
