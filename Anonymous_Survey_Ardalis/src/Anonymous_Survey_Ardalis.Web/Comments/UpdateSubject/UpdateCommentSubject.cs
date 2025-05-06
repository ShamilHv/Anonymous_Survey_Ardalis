using Anonymous_Survey_Ardalis.Core.Exceptions;
using Anonymous_Survey_Ardalis.UseCases.Comments.Commands.UpdateSubject;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Anonymous_Survey_Ardalis.Web.Comments.UpdateSubject;

[Authorize]
public class UpdateCommentSubject : Endpoint<UpdateCommentSubjectRequest, UpdateCommentSubjectResponse>
{
  private readonly IMediator _mediator;
  private readonly ICurrentUserService _currentUserService;

  public UpdateCommentSubject(IMediator mediator, ICurrentUserService currentUserService)
  {
    _mediator = mediator;
    _currentUserService = currentUserService;
  }

  public override void Configure()
  {
    Put(UpdateCommentSubjectRequest.Route);
    AllowFormData();
    Description(x => x
      .WithName("UpdateCommentSubject")
      .WithTags("Comments")
      .WithDescription("Updates the subject ID of a comment. Only super admins can perform this operation."));
    Summary(s =>
    {
      s.ExampleRequest = new UpdateCommentSubjectRequest { CommentId = 1, NewSubjectId = 2 };
    });
  }

  public override async Task HandleAsync(
    UpdateCommentSubjectRequest request,
    CancellationToken cancellationToken)
  {
    try
    {
      var adminId = _currentUserService.GetCurrentAdminId();
      var result = await _mediator.Send(
        new UpdateCommentSubjectCommand(request.CommentId, request.NewSubjectId, adminId), 
        cancellationToken);

      if (result.Status == ResultStatus.Forbidden)
      {
        await SendForbiddenAsync(cancellationToken);
        return;
      }

      if (result.IsSuccess)
      {
        Response = new UpdateCommentSubjectResponse(true, "Comment subject updated successfully");
      }
      else
      {
        Response = new UpdateCommentSubjectResponse(false, "Failed to update comment subject");
        await SendAsync(Response, 400, cancellationToken);
      }
    }
    catch (ResourceNotFoundException ex)
    {
      ThrowError(ex.Message);
      await SendErrorsAsync(404, cancellationToken);
    }
    catch (Exception ex)
    {
      ThrowError(ex.Message);
      await SendErrorsAsync(500, cancellationToken);
    }
  }
}
