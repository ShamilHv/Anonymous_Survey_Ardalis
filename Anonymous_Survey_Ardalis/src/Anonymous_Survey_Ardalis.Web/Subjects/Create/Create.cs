using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Anonymous_Survey_Ardalis.UseCases.Subjects.Commands.Create;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class Create(
  IMediator _mediator,
  IAdminPermissionService _adminPermissionService,
  ICurrentUserService _currentUserService)
  : Endpoint<CreateSubjectRequest, CreateSubjectResponse>
{
  public override void Configure()
  {
    Post(CreateSubjectRequest.Route);
    AllowFormData();
    Summary(s =>
    {
      s.ExampleRequest = new CreateSubjectCommand("Design", 2);
    });
  }

  public override async Task HandleAsync(
    CreateSubjectRequest request,
    CancellationToken cancellationToken)
  {
    var adminId = _currentUserService.GetCurrentAdminId();
    if (!await _adminPermissionService.CanCreateSubject(adminId, request.DepartmentId))
    {
      await SendForbiddenAsync();
      return;
    }

    var result = await _mediator.Send(new CreateSubjectCommand(request.SubjectName,
      request.DepartmentId), cancellationToken);
    if (result.IsNotFound())
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    {
    }
    if (result.IsSuccess)
    {
      Response = new CreateSubjectResponse(request.DepartmentId, request.SubjectName)
      {
        SubjectId = result.Value, CreatedAt = DateTime.UtcNow
      };
    }
  }
}
