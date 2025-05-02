using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Anonymous_Survey_Ardalis.UseCases.Departments.Commands.Create;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Anonymous_Survey_Ardalis.Web.Departments;

public class Create(
  IMediator _mediator,
  IAdminPermissionService _adminPermissionService,
  ICurrentUserService _currentUserService)
  : Endpoint<CreateDepartmentRequest, CreateDepartmentResponse>
{
  public override void Configure()
  {
    Post(CreateDepartmentRequest.Route);
    AllowFormData();
    Summary(s =>
    {
      s.ExampleRequest = new CreateDepartmentCommand("IT");
    });
  }

  [Authorize(Policy = "SuperAdminOnly")]
  public override async Task HandleAsync(
    CreateDepartmentRequest request,
    CancellationToken cancellationToken)
  {
    var adminId = _currentUserService.GetCurrentAdminId();
    if (!await _adminPermissionService.CanModifyDepartment(adminId))
    {
      await SendForbiddenAsync();
      return;
    }

    var result = await _mediator.Send(new CreateDepartmentCommand(request.DepartmentName), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new CreateDepartmentResponse
      {
        DepartmentId = result.Value, CreatedAt = DateTime.UtcNow, DepartmentName = request.DepartmentName
      };
    }
  }
}
