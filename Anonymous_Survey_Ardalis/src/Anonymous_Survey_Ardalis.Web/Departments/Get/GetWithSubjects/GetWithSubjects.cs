using Anonymous_Survey_Ardalis.UseCases.Departments;
using Anonymous_Survey_Ardalis.UseCases.Departments.Queries.GetWithSubjects;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Departments.Get.GetWithSubjects;

public class GetWithSubjects(IMediator _mediator)
  : Endpoint<GetDepartmentWithSubjectsRequest, DepartmentWithSubjectsDto>
{
  public override void Configure()
  {
    Get(GetDepartmentWithSubjectsRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetDepartmentWithSubjectsRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetDepartmentWithSubjectsQuery(request.DepartmentId);

    var result = await _mediator.Send(query, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = result;
    }
  }
}
