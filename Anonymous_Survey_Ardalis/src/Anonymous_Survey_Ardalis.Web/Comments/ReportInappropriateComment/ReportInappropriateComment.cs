using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.UseCases.Comments.Commands.ReportInappropriateComment;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Anonymous_Survey_Ardalis.Web.Comments.ReportInappropriateComment;

[Authorize]
public class ReportInappropriateComment : Endpoint<ReportInappropriateCommentRequest, ReportInappropriateCommentResponse>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public ReportInappropriateComment(
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public override void Configure()
    {
        Post(ReportInappropriateCommentRequest.Route);
        AllowFormData();
        Roles(AdminRole.SubjectAdmin.ToString(), AdminRole.DepartmentAdmin.ToString(), AdminRole.SuperAdmin.ToString());
        Summary(s =>
        {
            s.Summary = "Report a comment as inappropriate";
            s.Description = "Admins can report comments as inappropriate, which will notify super admins";
        });
    }

    public override async Task HandleAsync(ReportInappropriateCommentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new ReportInappropriateCommentCommand
            {
                CommentId = request.CommentId,
                Message = request.Message
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                Response = new ReportInappropriateCommentResponse
                {
                    Success = true,
                    Message = "Inappropriate comment report sent successfully"
                };
            }
            else
            {
                Response = new ReportInappropriateCommentResponse
                {
                    Success = false,
                    Message = result.Errors.FirstOrDefault() ?? "Failed to report inappropriate comment"
                };
                await SendAsync(Response, 400, cancellationToken);
                return;
            }
        }
        catch (Exception ex)
        {
            Response = new ReportInappropriateCommentResponse
            {
                Success = false,
                Message = ex.Message
            };
            await SendAsync(Response, 500, cancellationToken);
            return;
        }
        
        await SendAsync(Response);
    }
}
