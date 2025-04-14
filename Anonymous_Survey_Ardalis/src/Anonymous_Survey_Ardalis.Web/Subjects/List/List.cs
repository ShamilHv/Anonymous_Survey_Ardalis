using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.List;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class List(IMediator _mediator) : EndpointWithoutRequest<SubjectsListResponse>
{
  public override void Configure()
  {
    Get("/Subjects");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    var result =
      await _mediator.Send(new ListSubjectsQuery(), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new SubjectsListResponse
      {
        Subjects = result.Value.Select(s => new SubjectRecord(s.SubjectId, s.SubjectName, s.DepartmentId)).ToList()
      };
    }
  }
}
