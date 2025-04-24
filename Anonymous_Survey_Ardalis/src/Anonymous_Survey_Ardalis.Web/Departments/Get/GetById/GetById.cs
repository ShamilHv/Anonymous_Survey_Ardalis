using Anonymous_Survey_Ardalis.UseCases.Departments.Queries.Get;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Departments;

public class GetById(IMediator _mediator)
  : Endpoint<GetDepartmentByIdRequest, DepartmentRecord>
{
  public override void Configure()
  {
    Get(GetDepartmentByIdRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetDepartmentByIdRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetDepartmentQuery(request.DepartmentId);

    var result = await _mediator.Send(query, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new DepartmentRecord(result.Value.DepartmentId, result.Value.DepartmentName, result.Value.CreatedAt);
    }
  }
}
