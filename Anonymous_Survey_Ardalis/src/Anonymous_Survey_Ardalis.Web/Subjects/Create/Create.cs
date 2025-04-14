using Anonymous_Survey_Ardalis.UseCases.Subjects.Commands.Create;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class Create(IMediator _mediator)
  : Endpoint<CreateSubjectRequest, CreateSubjectResponse>
{
  public override void Configure()
  {
    Post(CreateSubjectRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.ExampleRequest = new CreateSubjectCommand("Design", 2);
    });
  }

  public override async Task HandleAsync(
    CreateSubjectRequest request,
    CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new CreateSubjectCommand(request.SubjectName,
      request.DepartmentId), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new CreateSubjectResponse(request.DepartmentId, request.SubjectName)
      {
        SubjectId = result.Value, CreatedAt = DateTime.UtcNow
      };
    }
  }
}
