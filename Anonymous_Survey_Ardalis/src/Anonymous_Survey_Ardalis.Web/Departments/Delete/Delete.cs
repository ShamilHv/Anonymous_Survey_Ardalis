using Anonymous_Survey_Ardalis.UseCases.Departments.Commands.Delete;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Departments;

public class Delete(IMediator _mediator)
  : Endpoint<DeleteDepartmentRequest>
{
  public override void Configure()
  {
    Delete(DeleteDepartmentRequest.Route);
  }

  public override async Task HandleAsync(
    DeleteDepartmentRequest request,
    CancellationToken cancellationToken)
  {
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
