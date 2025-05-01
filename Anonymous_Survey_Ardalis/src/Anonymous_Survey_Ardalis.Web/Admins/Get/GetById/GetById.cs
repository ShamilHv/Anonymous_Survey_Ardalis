using Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Admins.Get.GetById;

public class GetById(IMediator mediator) : Endpoint<GetAdminByIdRequest, AdminRecord>
{
  public override void Configure()
  {
    Get(GetAdminByIdRequest.Route);
  }

  public override async Task HandleAsync(GetAdminByIdRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetAdminQuery(request.AdminId);

    var result = await mediator.Send(query, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new AdminRecord(result.Value.Id, result.Value.AdminName, result.Value.Email, result.Value.SubjectId,
        result.Value.DepartmentId, result.Value.CreatedAt, result.Value.Role);
    }
  }
}
