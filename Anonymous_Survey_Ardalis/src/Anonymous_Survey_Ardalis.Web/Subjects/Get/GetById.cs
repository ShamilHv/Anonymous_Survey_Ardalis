using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class GetById(IMediator _mediator)
  : Endpoint<GetSubjectByIdRequest, SubjectRecord>
{
  public override void Configure()
  {
    Get(GetSubjectByIdRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetSubjectByIdRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetSubjectQuery(request.SubjectId);

    var result = await _mediator.Send(query, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new SubjectRecord(result.Value.SubjectId, result.Value.SubjectName, result.Value.DepartmentId);
    }
  }
}
