using Anonymous_Survey_Ardalis.UseCases.Departments.Queries.List;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Departments;

public class List(IMediator _mediator) : EndpointWithoutRequest<DepartmentListResponse>
{
  public override void Configure()
  {
    Get("/Departments");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    var result =
      await _mediator.Send(new ListDepartmentsQuery(), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new DepartmentListResponse
      {
        Departments = result.Value.Select(d => new DepartmentRecord(d.DepartmentId, d.DepartmentName, d.CreatedAt))
          .ToList()
      };
    }
  }
}
