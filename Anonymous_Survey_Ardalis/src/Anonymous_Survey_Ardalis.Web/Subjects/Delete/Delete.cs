using Anonymous_Survey_Ardalis.UseCases.Subjects.Commands.Delete;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class Delete(IMediator _mediator)
  : Endpoint<DeleteSubjectRequest>
{
  public override void Configure()
  {
    Delete(DeleteSubjectRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
    DeleteSubjectRequest request,
    CancellationToken cancellationToken)
  {
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
