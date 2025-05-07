using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.MarkAsInappropriate;

public class MarkCommentAsInappropriateHandler : IRequestHandler<MarkCommentAsInappropriateCommand, Result<bool>>
{
  private readonly IRepository<Comment> _commentRepository;
  private readonly ICurrentUserService _currentUserService;
  private readonly IAdminPermissionService _permissionService;

  public MarkCommentAsInappropriateHandler(
    IRepository<Comment> commentRepository,
    ICurrentUserService currentUserService,
    IAdminPermissionService permissionService)
  {
    _commentRepository = commentRepository;
    _currentUserService = currentUserService;
    _permissionService = permissionService;
  }

  public async Task<Result<bool>> Handle(MarkCommentAsInappropriateCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var adminId = _currentUserService.GetCurrentAdminId();

      // Get the comment
      var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
      if (comment == null)
      {
        return Result<bool>.Error("Comment not found");
      }

      // Check permission if admin can mark this comment as inappropriate
      var hasPermission = await _permissionService.CanMarkCommentAsInappropriate(adminId, comment.Id);
      if (!hasPermission)
      {
        return Result<bool>.Error("You don't have permission to mark this comment as inappropriate");
      }

      // Update the comment
      comment.IsAppropriate = false;
      await _commentRepository.UpdateAsync(comment, cancellationToken);

      return Result<bool>.Success(true);
    }
    catch (Exception ex)
    {
      return Result<bool>.Error(ex.Message);
    }
  }
}
