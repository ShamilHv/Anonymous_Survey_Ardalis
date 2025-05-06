using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.UseCases.Comments.Commands.RequestSubjectChange;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Anonymous_Survey_Ardalis.Web.Comments.RequestSubjectChange;

[Authorize]
public class RequestSubjectChange : Endpoint<RequestSubjectChangeRequest, RequestSubjectChangeResponse>
{
  private readonly ICurrentUserService _currentUserService;
  private readonly IMediator _mediator;

  public RequestSubjectChange(
    IMediator mediator,
    ICurrentUserService currentUserService)
  {
    _mediator = mediator;
    _currentUserService = currentUserService;
  }

  public override void Configure()
  {
    Post(RequestSubjectChangeRequest.Route);
    AllowFormData();
    Roles(AdminRole.SubjectAdmin.ToString(), AdminRole.DepartmentAdmin.ToString(), AdminRole.SuperAdmin.ToString());
    Summary(s =>
    {
      s.Summary = "Request to change a comment's subject";
      s.Description =
        "Subject admins can request to change a comment's subject if it was submitted under the wrong subject";
    });
  }

  public override async Task HandleAsync(RequestSubjectChangeRequest request, CancellationToken cancellationToken)
  {
    try
    {
      var command = new RequestSubjectChangeCommand
      {
        CommentId = request.CommentId, SuggestedSubjectId = request.SuggestedSubjectId, Message = request.Message
      };

      var result = await _mediator.Send(command, cancellationToken);

      if (result.IsSuccess)
      {
        Response = new RequestSubjectChangeResponse
        {
          Success = true, Message = "Subject change request sent successfully"
        };
      }
      else
      {
        Response = new RequestSubjectChangeResponse
        {
          Success = false, Message = result.Errors.FirstOrDefault() ?? "Failed to send subject change request"
        };
        await SendAsync(Response, 400, cancellationToken);
      }
    }
    catch (Exception ex)
    {
      Response = new RequestSubjectChangeResponse { Success = false, Message = ex.Message };
      await SendAsync(Response, 500, cancellationToken);
    }
  }
}
