using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Anonymous_Survey_Ardalis.UseCases.Subjects.Commands.Delete;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class Delete(
  IMediator _mediator,
  IAdminPermissionService _adminPermissionService,
  ICurrentUserService _currentUserService)
  : Endpoint<DeleteSubjectRequest>
{
  public override void Configure()
  {
    Delete(DeleteSubjectRequest.Route);
  }

  public override async Task HandleAsync(
    DeleteSubjectRequest request,
    CancellationToken cancellationToken)
  {
    var adminId = _currentUserService.GetCurrentAdminId();
    if (!await _adminPermissionService.CanDeleteSubject(adminId, request.Subjectid))
    {
      await SendForbiddenAsync();
      return;
    }

    var command = new DeleteSubjectCommand(request.Subjectid);

    var result = await _mediator.Send(command, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      await SendNoContentAsync(cancellationToken);
    }

    ;
  }
}
