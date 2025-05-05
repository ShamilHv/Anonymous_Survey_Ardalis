using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.RequestSubjectChange;

public class RequestSubjectChangeHandler: IRequestHandler<RequestSubjectChangeCommand, Result<bool>>
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAdminPermissionService _permissionService;
    private readonly IEmailSender _emailSender;

    public RequestSubjectChangeHandler(
        IRepository<Comment> commentRepository,
        ICurrentUserService currentUserService,
        IAdminPermissionService permissionService,
        IEmailSender emailSender)
    {
        _commentRepository = commentRepository;
        _currentUserService = currentUserService;
        _permissionService = permissionService;
        _emailSender = emailSender;
    }

    public async Task<Result<bool>> Handle(RequestSubjectChangeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var adminId = _currentUserService.GetCurrentAdminId();
            var adminName = _currentUserService.GetCurrentAdminName();
            var adminRole = _currentUserService.GetCurrentAdminRole();
            
            // Get the comment
            var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
            if (comment == null)
            {
                return Result<bool>.Error("Comment not found");
            }

            // Check permission if admin can access this comment
            bool hasAccess = await _permissionService.CanRequestSubjectChange(adminId, comment.Id);
            if (!hasAccess)
            {
                return Result<bool>.Error("You don't have permission to request changes for this comment");
            }

            // Get admin details for email
            var admin = await _currentUserService.GetCurrentAdminEntityAsync();
            if (admin == null)
            {
                return Result<bool>.Error("Admin information could not be retrieved");
            }

            // Send email to super admins
            await _emailSender.SendSubjectChangeRequestEmailAsync(
                admin,
                comment,
                request.SuggestedSubjectId,
                request.Message);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }
}
