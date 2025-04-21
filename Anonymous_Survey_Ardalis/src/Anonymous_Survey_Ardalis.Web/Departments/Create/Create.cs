using Anonymous_Survey_Ardalis.UseCases.Departments.Commands.Create;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Departments;

public class Create(IMediator _mediator)
  : Endpoint<CreateDepartmentRequest, CreateDepartmentResponse>
{
  public override void Configure()
  {
    Post(CreateDepartmentRequest.Route);
    Summary(s =>
    {
      s.ExampleRequest = new CreateDepartmentCommand("IT");
    });
  }

  public override async Task HandleAsync(
    CreateDepartmentRequest request,
    CancellationToken cancellationToken)
  {
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
