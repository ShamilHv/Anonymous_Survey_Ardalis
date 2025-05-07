using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.UseCases.Comments.Commands.MarkAsInappropriate;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Anonymous_Survey_Ardalis.Web.Comments.MarkCommentAsInappropriate;

[Authorize]
public class
  MarkCommentAsInappropriate : Endpoint<MarkCommentAsInappropriateRequest, MarkCommentAsInappropriateResponse>
{
  private readonly ICurrentUserService _currentUserService;
  private readonly IMediator _mediator;

  public MarkCommentAsInappropriate(
    IMediator mediator,
    ICurrentUserService currentUserService)
  {
    _mediator = mediator;
    _currentUserService = currentUserService;
  }

  public override void Configure()
  {
    Post(MarkCommentAsInappropriateRequest.Route);
    AllowFormData();
    Roles(AdminRole.SuperAdmin.ToString()); // Only SuperAdmin can mark comments as inappropriate
    Summary(s =>
    {
      s.Summary = "Mark a comment as inappropriate";
      s.Description = "Super admins can mark comments as inappropriate after reviewing reports";
    });
  }

  public override async Task HandleAsync(MarkCommentAsInappropriateRequest request, CancellationToken cancellationToken)
  {
    try
    {
      var command = new MarkCommentAsInappropriateCommand { CommentId = request.CommentId };

      var result = await _mediator.Send(command, cancellationToken);

      if (result.IsSuccess)
      {
        Response = new MarkCommentAsInappropriateResponse
        {
          Success = true, Message = "Comment marked as inappropriate successfully"
        };
      }
      else
      {
        Response = new MarkCommentAsInappropriateResponse
        {
          Success = false, Message = result.Errors.FirstOrDefault() ?? "Failed to mark comment as inappropriate"
        };
        await SendAsync(Response, 400, cancellationToken);
        return;
      }
    }
    catch (Exception ex)
    {
      Response = new MarkCommentAsInappropriateResponse { Success = false, Message = ex.Message };
      await SendAsync(Response, 500, cancellationToken);
      return;
    }

    await SendAsync(Response);
  }
}
