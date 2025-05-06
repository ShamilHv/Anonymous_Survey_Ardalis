using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;
using Anonymous_Survey_Ardalis.Core.Exceptions;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate.Specifications;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.UpdateSubject;

public class UpdateCommentSubjectHandler : ICommandHandler<UpdateCommentSubjectCommand, Result<bool>>
{
  private readonly IRepository<Comment> _commentRepository;
  private readonly IRepository<Subject> _subjectRepository;
  private readonly IAdminPermissionService _adminPermissionService;

  public UpdateCommentSubjectHandler(
    IRepository<Comment> commentRepository,
    IRepository<Subject> subjectRepository,
    IAdminPermissionService adminPermissionService)
  {
    _commentRepository = commentRepository;
    _subjectRepository = subjectRepository;
    _adminPermissionService = adminPermissionService;
  }

  public async Task<Result<bool>> Handle(UpdateCommentSubjectCommand request, CancellationToken cancellationToken)
  {
    // Check if admin has permission
    if (!await _adminPermissionService.CanUpdateCommentSubject(request.AdminId))
    {
      return Result<bool>.Forbidden();
    }

    // Check if comment exists
    var commentSpec = new CommentByIdSpec(request.CommentId);
    var comment = await _commentRepository.FirstOrDefaultAsync(commentSpec, cancellationToken);
    if (comment == null)
    {
      throw new ResourceNotFoundException($"Comment id {request.CommentId}");
    }

    // Check if subject exists
    var subjectSpec = new SubjectByIdSpec(request.NewSubjectId);
    var subject = await _subjectRepository.FirstOrDefaultAsync(subjectSpec, cancellationToken);
    if (subject == null)
    {
      throw new ResourceNotFoundException($"Subject id {request.NewSubjectId}");
    }

    // Update the comment's subject
    comment.SubjectId = request.NewSubjectId;
    await _commentRepository.UpdateAsync(comment, cancellationToken);
    await _commentRepository.SaveChangesAsync(cancellationToken);

    return Result<bool>.Success(true);
  }
}
