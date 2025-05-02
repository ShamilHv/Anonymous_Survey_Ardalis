using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Anonymous_Survey_Ardalis.UseCases.Departments.Commands.Delete;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Anonymous_Survey_Ardalis.Web.Departments;

public class Delete(
  IMediator _mediator,
  IAdminPermissionService _adminPermissionService,
  ICurrentUserService _currentUserService)
  : Endpoint<DeleteDepartmentRequest>
{
  public override void Configure()
  {
    Delete(DeleteDepartmentRequest.Route);
  }

  [Authorize(Policy = "DepartmentAdminOrHigher")]
  public override async Task HandleAsync(
    DeleteDepartmentRequest request,
    CancellationToken cancellationToken)
  {
    var adminId = _currentUserService.GetCurrentAdminId();
    if (!await _adminPermissionService.CanModifyDepartment(adminId))
    {
      await SendForbiddenAsync();
      return;
    }

    var command = new DeleteDepartmentCommand(request.DepartmentId);

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
